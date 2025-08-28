using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Vneed.Common.Helpers;
using Vneed.Services.Dto;
using Vneed.Services.Interfaces;
using Vneed.Common.Constants;

namespace Vneed.Api.Controllers
{
    [ApiController]
    [Route("api/")]
    [Authorize(Roles = "3")]
    public class UserController(
        IUserService userService,
        IAuthService authService,
        IUserMailTokenService userMailTokenService
    ) : ControllerBase
    {
        
        // Kullanıcıları filtreleyerek (aktiflik ve role göre) listeleme
        [HttpGet("user")]
        public async Task<IActionResult> GetUsers([FromQuery] bool? isActive, [FromQuery] int? roleId)
        {
            var users = await userService.GetAll();

            if (isActive.HasValue)
                users = users.Where(u => u.IsActive == isActive.Value).ToList();

            if (roleId.HasValue)
                users = users.Where(u => u.RoleId == roleId.Value).ToList();

            return Ok(ApiResponseHelper.SuccessWithData(users));
        }
        
        //UserId ye göre kullanıcı getirme
        [HttpGet("user/{id}")]
        public async Task<IActionResult> GetUserById(int id)
        {
            var user = await userService.GetById(id);
            if (user == null)
            {
                return NotFound(ApiResponseHelper.Fail<object>(
                    "not_found",
                    id + " numaralı kullanıcı bulunamadı."));
            }
            return Ok(ApiResponseHelper.SuccessWithData(user));
        } 
        
        // Kullanıcıları sayfalı olarak getirir.
        
        [HttpGet("paged")]
        [Authorize]
        public async Task<IActionResult> GetPagedUsers(
            [FromQuery] int pageNumber = PaginationDefaults.DefaultPageNumber,
            [FromQuery] int pageSize = PaginationDefaults.DefaultPageSize)
        {
            try
            {
                var result = await userService.GetPagedAsync(pageNumber, pageSize);
                return Ok(ApiResponseHelper.SuccessWithData(result, "Kullanıcılar başarıyla listelendi."));
            }
            catch (ArgumentException ex)
            {
                // Hata türüne göre kod belirleniyor.
                var errorCode = ex is ArgumentOutOfRangeException ? "out_of_range" : "invalid_paging";
                return BadRequest(ApiResponseHelper.Fail<object>(errorCode, ex.Message));
            }
        }
        

        // Admin User oluşturma
        [HttpPost("user")]
        public async Task<IActionResult> CreateUserByAdmin([FromBody] UserCreateByAdminDto dto)
        {
            var existingUser = await userService.GetByEmail(dto.Email);
            if (existingUser != null)
            {
                return BadRequest(ApiResponseHelper.Fail<object>("email_exists", "Bu e-posta adresi zaten kayıtlı."));
            }

            var created = await userService.CreateUserByAdmin(dto);
            if (!created)
                return BadRequest(ApiResponseHelper.Fail<object>("create_failed", "Kullanıcı oluşturulamadı."));

            var user = await userService.GetByEmail(dto.Email);
            if (user == null)
            {
                return BadRequest(ApiResponseHelper.Fail<object>("user_not_found", "Oluşturulan kullanıcı bulunamadı."));
            }

            var token = await userMailTokenService.CreateToken(user.Id);
            var resetLink = "https://valde.co/" + token.ResetKey;

            var emailResponse = await authService.SendResetPasswordEmail(dto.Email, resetLink);

            if (!emailResponse.Success)
            {
                return BadRequest(ApiResponseHelper.Fail<object>("email_failed", "Şifre sıfırlama maili gönderilemedi."));
            }

            return Ok(ApiResponseHelper.SuccessMessage("Kullanıcı başarıyla oluşturuldu ve şifre sıfırlama maili gönderildi."));
        }
        
        //Kullanıcı Güncelleme
        [HttpPut("user/{id}")]
        public async Task<IActionResult> UpdateUser(int id, [FromBody] UserUpdateByAdminDto dto)
        {
            var result = await userService.UpdateByAdmin(id, dto);

            if (!result)
            {
                return NotFound(ApiResponseHelper.Fail<object>(
                    "not_found",
                    "User with id " + id + " not found."));
            }

            return Ok(ApiResponseHelper.SuccessMessage("Kullanıcı başarıyla güncellendi."));
        }
        
        //Kullanıcı Silme
        [HttpDelete("user/{id}")]
        public async Task<IActionResult> DeleteUser(int id)
        {
            var success = await userService.Delete(id);
            if (!success)
            {
                return NotFound(ApiResponseHelper.Fail<object>(
                    "not_found",
                    id + " numaralı kullanıcı bulunamadı veya silinemedi."));
            }
            return Ok(ApiResponseHelper.SuccessMessage("Kullanıcı başarıyla silindi."));
        }
    }
}
