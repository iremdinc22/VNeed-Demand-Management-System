using Vneed.Common.Models;

namespace Vneed.Common.Helpers;

public static class ApiResponseHelper
{
    
    //Get işlemlerinde kullanıldı
    public static ApiResponse<T> SuccessWithData<T>(T data, string message = "Başarılı")
    {
        return new ApiResponse<T>
        {
            Success = true,
            Timestamp = DateTimeOffset.UtcNow.ToUnixTimeSeconds(),
            Message = message,
            Result = data
        };
    }    
    
    //Post, Delete, Put işlemlerinde kullanıldı
    public static ApiResponse<object> SuccessMessage(string? message)
    {
        return new ApiResponse<object>
        {
            Success = true,
            Timestamp = DateTimeOffset.UtcNow.ToUnixTimeSeconds(),
            Message = message
        };
    }

    
    public static ApiResponse<T> Fail<T>(string code, string message, Exception? ex = null)
    {
        return new ApiResponse<T>
        {
            Success = false,
            Timestamp = DateTimeOffset.UtcNow.ToUnixTimeSeconds(),
            Error = new ApiError
            {
                Code = code,
                Message = message,
                Detail = ex?.ToString()
            }
        };
    }
    
    public static ApiResponse<List<ApiValidationErrorDetail>> ValidationFail(List<ApiValidationErrorDetail> errors)
    {
        return new ApiResponse<List<ApiValidationErrorDetail>>
        {
            Success = false,
            Timestamp = DateTimeOffset.UtcNow.ToUnixTimeSeconds(),
            Message = "Doğrulama hatası oluştu",
            Result = errors,
            Error = new ApiError
            {
                Code = "validation_error",
                Message = "Doğrulama hatası oluştu"
            }
        };
    }
}