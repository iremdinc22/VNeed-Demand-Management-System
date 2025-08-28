using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Org.BouncyCastle.Crypto.Generators;
using Vneed.Services.Interfaces;
using Vneed.Services.Dto;
using Vneed.Common.Helpers;
using Vneed.Common.Models;

namespace Vneed.Api.Controllers
{
    [ApiController]
    [Route("api/auth")]
    public class AuthController(IUserService userService,
        IAuthService authService,
        IUserMailTokenService userMailTokenService) : ControllerBase
    {
        
        // Giriş işlemi
        [HttpPost("login")]
        [AllowAnonymous]
        public async Task<IActionResult> Login([FromBody] LoginRequestDto request)
        {
            var response = await authService.Login(request.Email, request.Password);

            if (!response.Success)
            {
                return Unauthorized(response);
            }
            return Ok(response);
        }
        
        
        // Şifre belirleme - token query parametre olarak alınıyor
        [HttpPost("reset-password")]
        [AllowAnonymous]
        public async Task<IActionResult> SetPassword([FromQuery] string token, [FromBody] SetPasswordRequestDto request)
        {
            if (request.NewPassword != request.ConfirmPassword)
            {
                return BadRequest(ApiResponseHelper.Fail<object>("password_mismatch", "Şifreler uyuşmuyor."));
            }

            var result = await authService.SetPassword(token, request.NewPassword);
            if (!result.Success)
            {
                return BadRequest(result);
            }

            return Ok(result);
        }
        
        // Şifremi unuttum
        [HttpPost("forgot-password")]
        [AllowAnonymous]
        public async Task<IActionResult> ForgotPassword([FromBody] ForgotPasswordDto dto)
        {
            var user = await userService.GetByEmail(dto.Email);

            if (user != null)
            {
                var token = await userMailTokenService.CreateToken(user.Id);
                var resetLink = "https://valde.co/" + token.ResetKey;

                await authService.SendResetPasswordEmail(user.Email, resetLink);
            }

            return Ok(ApiResponseHelper.SuccessMessage("Eğer bu e-posta sistemimizde kayıtlıysa, şifre sıfırlama bağlantısı gönderilmiştir."));
        }
        
        //Çıkış yapma
        [HttpPost("logout")]
        [Authorize]
        public async Task<IActionResult> Logout()
        {
            var token = Request.Headers["Authorization"].ToString().Replace("Bearer ", "");
            if (string.IsNullOrEmpty(token))
            {
                return BadRequest(ApiResponseHelper.Fail<object>("missing_token", "Token bulunamadı."));
            }

            var result = await authService.Logout(token);
            if (!result.Success)
                return BadRequest(result);

            return Ok(result);
        }
    }
}