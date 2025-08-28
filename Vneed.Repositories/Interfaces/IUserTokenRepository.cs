using System.Linq.Expressions;
using Vneed.Data.Models;

namespace Vneed.Repositories.Interfaces;

public interface IUserTokenRepository : IGenericRepository<UserToken>
{
    Task<UserToken> GetByToken(string token);
    Task<UserToken> GetByUserId(int userId);
    Task<IEnumerable<UserToken>> GetAll(Expression<Func<UserToken, bool>> predicate);
    Task<bool> UpdateWithTracking(UserToken token);
}