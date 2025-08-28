using Vneed.Data.Models;

namespace Vneed.Repositories.Interfaces;

public interface IUserRepository:IGenericRepository<User>
{
    Task<User> GetByEmail(string email);
}