using Microsoft.EntityFrameworkCore;
using Vneed.Data.Context;
using Vneed.Data.Models;
using Vneed.Repositories.Interfaces;

namespace Vneed.Repositories.Repositories;

public class UserMailTokenRepository : GenericRepository<UserMailToken>, IUserMailTokenRepository
{
    private readonly AppDbContext _context;

    public UserMailTokenRepository(AppDbContext context) : base(context)
    {
        _context = context;
    }

    public async Task<UserMailToken?> GetValidTokenAsync(string token)
    {
        return await _context.UserMailTokens
            .Include(t => t.User)
            .FirstOrDefaultAsync(t =>
                t.ResetKey == token &&
                !t.IsUsed &&
                t.ExpiresAt > DateTime.UtcNow);
    }
}