using Vneed.Common;
using Vneed.Common.Models;
using Vneed.Services.Dto;

namespace Vneed.Services.Interfaces;

public interface IAuthService
{
    Task<ApiResponse<AuthResponseDto>> Login(string email, string password);
    Task<ApiResponse<object>> ResetPassword(string token, string newPassword, string confirmPassword);
    Task<ApiResponse<object>> SendResetPasswordEmail(string toEmail, string resetLink);
    Task<ApiResponse<object>> Logout(string token);
    Task<ApiResponse<object>> SetPassword(string token, string newPassword);
}