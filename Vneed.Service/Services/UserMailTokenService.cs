using Vneed.Data.Models;
using Vneed.Repositories.Interfaces;
using Vneed.Services.Interfaces;

namespace Vneed.Services.Services;

public class UserMailTokenService : IUserMailTokenService
{
    private readonly IUserMailTokenRepository _tokenRepository;

    public UserMailTokenService(IUserMailTokenRepository tokenRepository)
    {
        _tokenRepository = tokenRepository;
    }

    public async Task<UserMailToken?> GetValidToken(string token)
    {
        return await _tokenRepository.GetValidTokenAsync(token);
    }

    private static readonly Random _random = new();

    private string GenerateRandomToken(int length = 10)
    {
        const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
        return new string(Enumerable.Repeat(chars, length)
            .Select(s => s[_random.Next(s.Length)]).ToArray());
    }

    public async Task<UserMailToken> CreateToken(int userId)
    {
        var token = new UserMailToken
        {
            UserId = userId,
            ResetKey = Guid.NewGuid().ToString("N"), 
            IsUsed = false,
            CreatedAt = DateTime.UtcNow,
            ExpiresAt = DateTime.UtcNow.AddHours(1)
        };

        var addedToken = await _tokenRepository.Add(token);

        if (addedToken == null)
            throw new Exception("Token veritabanına kaydedilemedi!");

        return addedToken;
    }


    public async Task MarkTokenAsUsed(UserMailToken token)
    {
        token.IsUsed = true;
        await _tokenRepository.Update(token);
    }
}