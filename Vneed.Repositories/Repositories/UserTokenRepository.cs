using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;
using Vneed.Data.Context;
using Vneed.Data.Models;
using Vneed.Repositories.Interfaces;

namespace Vneed.Repositories.Repositories;

public class UserTokenRepository : GenericRepository<UserToken>, IUserTokenRepository
{
    private readonly AppDbContext _context;

    public UserTokenRepository(AppDbContext context) : base(context)
    {
        _context = context;
    }

    public async Task<UserToken> GetByToken(string token)
    {
        return await _context.UserToken.FirstOrDefaultAsync(x => x.Token == token);
    }

    public async Task<UserToken> GetByUserId(int userId)
    {
        return await _context.UserToken.FirstOrDefaultAsync(x => x.UserId == userId);
    }
    
    public async Task<IEnumerable<UserToken>> GetAll(Expression<Func<UserToken, bool>> predicate)
    {
        return await _context.UserToken.Where(predicate).ToListAsync();
    }
    
    public async Task<bool> UpdateWithTracking(UserToken token)
    {
        var trackedEntity = _context.ChangeTracker.Entries<UserToken>()
            .FirstOrDefault(e => e.Entity.Id == token.Id);

        if (trackedEntity == null)
        {
            _context.UserToken.Attach(token);
            _context.Entry(token).State = EntityState.Modified;
        }
        else
        {
            trackedEntity.CurrentValues.SetValues(token);
        }
        return await _context.SaveChangesAsync() > 0;
    }
}