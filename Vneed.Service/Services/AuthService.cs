using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using Vneed.Data.Models;
using Vneed.Services.Dto;
using Vneed.Services.Interfaces;
using MailKit.Net.Smtp;
using MimeKit;
using Vneed.Common.Helpers;
using Vneed.Common.Models;
using Amazon;
using Amazon.SimpleEmail;
using Amazon.SimpleEmail.Model;
using Vneed.Common.Helpers.HashPassword;


public class AuthService : IAuthService
{
    private readonly IConfiguration _configuration;
    private readonly IUserService _userService;
    private readonly IUserTokenService _userTokenService;
    private readonly IUserMailTokenService _userMailTokenService; 

    public AuthService(
        IConfiguration configuration, 
        IUserService userService, 
        IUserTokenService userTokenService,
        IUserMailTokenService userMailTokenService)
    {
        _configuration = configuration;
        _userService = userService;
        _userTokenService = userTokenService;
        _userMailTokenService = userMailTokenService;
    }

    public async Task<ApiResponse<AuthResponseDto>> Login(string email, string password)
    {
        var user = await _userService.GetByEmail(email);
        if (user == null || user.PasswordHash != HashHelper.ComputeSha256Hash(password))
        {
            return ApiResponseHelper.Fail<AuthResponseDto>("unauthorized", "E-posta veya şifre hatalı.");
        }

        if (!user.IsActive)
        {
            return ApiResponseHelper.Fail<AuthResponseDto>("inactive_user", "Kullanıcı pasif durumda, giriş yapamaz.");
        }
        
        var activeTokens = await _userTokenService.GetAll(t => t.UserId == user.Id && t.IsActive);
        foreach (var oldToken in activeTokens)
        {
            oldToken.IsActive = false;
            await _userTokenService.Update(oldToken);
        }
    
        var token = GenerateJwtToken(user);
        var expires = DateTime.UtcNow.AddMinutes(_configuration.GetValue<int>("JwtSettings:ExpiryMinutes"));
    
        await _userTokenService.Create(new UserTokenDto
        {
            UserId = user.Id,
            Token = token,
            CreatedAt = DateTime.UtcNow,
            ExpiresAt = expires,
            IsActive = true
        });

        var response = new AuthResponseDto
        {
            User = new LoginDto()
            {
                Id = user.Id,
                FullName = user.FullName ?? string.Empty,
                Email = user.Email ?? string.Empty,
                RoleId = user.RoleId,
                RoleName = user.RoleName,
                Token = token
            }
        };

        return ApiResponseHelper.SuccessWithData(response, "Giriş başarılı.");
    }
    
   
   // public string ComputeSha256Hash(string rawData)
    //{
      //  using var sha256 = SHA256.Create();
      //  var bytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(rawData));
       // var sb = new StringBuilder();
       // foreach (var b in bytes)
         //   sb.Append(b.ToString("x2"));
       // return sb.ToString();
   // }

    public string GenerateJwtToken(UserDto user)
    {
        var jwtSettings = _configuration.GetSection("JwtSettings");

        var claims = new[]
        {
            new Claim(JwtRegisteredClaimNames.Sub, user.Email),
            new Claim("userId", user.Id.ToString()),
            new Claim(ClaimTypes.Role, user.RoleId.ToString()),
            new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
        };

        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtSettings["SecretKey"]));
        var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

        var token = new JwtSecurityToken(
            issuer: jwtSettings["Issuer"],
            audience: jwtSettings["Audience"],
            claims: claims,
            expires: DateTime.Now.AddMinutes(jwtSettings.GetValue<int>("ExpiryMinutes")),
            signingCredentials: creds);

        return new JwtSecurityTokenHandler().WriteToken(token);
    }

    public async Task<ApiResponse<object>> SendResetPasswordEmail(string toEmail, string resetLink)
    {
        var user = await _userService.GetByEmail(toEmail);

        if (user != null)
        {
            var awsAccessKey = _configuration["AWS:AccessKey"];
            var awsSecretKey = _configuration["AWS:SecretKey"];
            var region = _configuration["AWS:Region"];
            var emailFrom = _configuration["AWS:EmailFrom"];

            var regionEndpoint = RegionEndpoint.GetBySystemName(region);

            using var client = new AmazonSimpleEmailServiceClient(awsAccessKey, awsSecretKey, regionEndpoint);

            var sendRequest = new SendEmailRequest
            {
                Source = emailFrom,
                Destination = new Destination
                {
                    ToAddresses = new List<string> { toEmail }
                },
                Message = new Message
                {
                    Subject = new Content("Şifre Sıfırlama"),
                    Body = new Body
                    {
                        Html = new Content("<p>Şifrenizi sıfırlamak için aşağıdaki linke tıklayın:</p><a href='" + resetLink + "'>Şifreyi Sıfırla</a>")
                    }
                }
            };

            try
            {
                await client.SendEmailAsync(sendRequest);
            }
            catch (Exception ex)
            {
                return ApiResponseHelper.Fail<object>("email_send_error", ex.Message);
            }
        }

        return ApiResponseHelper.SuccessMessage("Eğer böyle bir e-posta varsa, şifre sıfırlama bağlantısı gönderildi.");
    }

    public async Task<ApiResponse<object>> ResetPassword(string token, string newPassword, string confirmPassword)
    {
        if(newPassword != confirmPassword)
            return ApiResponseHelper.Fail<object>("password_mismatch", "Şifreler uyuşmuyor.");

        var tokenRecord = await _userMailTokenService.GetValidToken(token);
        if (tokenRecord == null)
        {
            return ApiResponseHelper.Fail<object>("invalid_token", "Geçersiz veya süresi dolmuş bağlantı.");
        }

        var passwordHash = HashHelper.ComputeSha256Hash(newPassword);

        var success = await _userService.UpdatePassword(tokenRecord.UserId, passwordHash);
        if (!success)
        {
            return ApiResponseHelper.Fail<object>("update_failed", "Şifre güncellenemedi.");
        }

        await _userMailTokenService.MarkTokenAsUsed(tokenRecord);

        return ApiResponseHelper.SuccessMessage("Şifre başarıyla oluşturuldu.");
    }
    

    public async Task<ApiResponse<object>> SetPassword(string token, string password)
    {
        var tokenRecord = await _userMailTokenService.GetValidToken(token);
        if (tokenRecord == null)
        {
            return ApiResponseHelper.Fail<object>("invalid_token", "Geçersiz veya süresi dolmuş bağlantı.");
        }

        var passwordHash = HashHelper.ComputeSha256Hash(password);

        var success = await _userService.UpdatePassword(tokenRecord.UserId, passwordHash);
        if (!success)
        {
            return ApiResponseHelper.Fail<object>("update_failed", "Şifre güncellenemedi.");
        }

        await _userMailTokenService.MarkTokenAsUsed(tokenRecord);

        return ApiResponseHelper.SuccessMessage("Şifre başarıyla oluşturuldu.");
    }
    
    public async Task<ApiResponse<object>> Logout(string token)
    {
        var existingToken = await _userTokenService.GetByToken(token);
        if (existingToken == null || !existingToken.IsActive)
        {
            return ApiResponseHelper.Fail<object>("invalid_token", "Geçersiz ya da zaten pasif token.");
        }

        existingToken.IsActive = false;
        await _userTokenService.Update(existingToken); 

        return ApiResponseHelper.SuccessMessage("Çıkış başarılı.");
    }
    
}
