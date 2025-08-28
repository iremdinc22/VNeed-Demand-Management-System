using System.Text.Json;
using System.Text.Json.Serialization;
using Microsoft.AspNetCore.Http;
using Vneed.Common.Helpers;
using Vneed.Common.Models;
using Vneed.Common.Exceptions;

namespace Vneed.Common.Middlewares;

public class GlobalExceptionHandlingMiddleware
{
    private readonly RequestDelegate _next;

    public GlobalExceptionHandlingMiddleware(RequestDelegate next)
    {
        _next = next;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        try
        {
            await _next(context);
        }
        catch (NotFoundException nfEx)
        {
            await WriteErrorResponse(context, 404, "not_found", nfEx.Message, nfEx);
            return;
        }
        catch (ValidationException valEx)
        {
            await WriteErrorResponse(context, 400, "validation_error", "Doğrulama hatası oluştu", valEx);
            return;
        }
        catch (Exception ex)
        {
            await WriteErrorResponse(context, 500, "internal_error", "Beklenmeyen hata oluştu", ex);
            return;
        }

        if (!context.Response.HasStarted)
        {
            if (context.Response.StatusCode == 401)
            {
                await WriteErrorResponse(context, 401, "unauthorized", "Yetkilendirme başarısız.");
            }
            else if (context.Response.StatusCode == 403)
            {
                await WriteErrorResponse(context, 403, "forbidden", "Erişim engellendi.");
            }
        }
    }

    private async Task WriteErrorResponse(HttpContext context, int statusCode, string code, string message, Exception? ex = null)
    {
        context.Response.ContentType = "application/json";
        context.Response.StatusCode = statusCode;

        var error = new ApiError
        {
            Code = code,
            Message = message
        };

#if DEBUG
        if (ex != null)
        {
            error.Detail = ex.ToString();
        }
#endif

        var response = new ApiResponse<object>
        {
            Success = false,
            Timestamp = DateTimeOffset.UtcNow.ToUnixTimeMilliseconds(),
            Message = null,
            Result = null,
            Error = error
        };

        var json = JsonSerializer.Serialize(response, new JsonSerializerOptions
        {
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
            DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull
        });

        await context.Response.WriteAsync(json);
    }
}