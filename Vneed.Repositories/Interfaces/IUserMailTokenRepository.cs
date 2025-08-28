using Vneed.Data.Models;

namespace Vneed.Repositories.Interfaces
{
    public interface IUserMailTokenRepository : IGenericRepository<UserMailToken>
    {
        Task<UserMailToken?> GetValidTokenAsync(string token);
    }
}
