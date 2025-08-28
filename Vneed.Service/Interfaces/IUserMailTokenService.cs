using Vneed.Data.Models;

namespace Vneed.Services.Interfaces;

public interface IUserMailTokenService
{
    Task<UserMailToken?> GetValidToken(string token);
    Task<UserMailToken> CreateToken(int userId);
    Task MarkTokenAsUsed(UserMailToken token);
}