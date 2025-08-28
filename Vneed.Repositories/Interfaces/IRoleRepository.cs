using Vneed.Data.Models;

namespace Vneed.Repositories.Interfaces;

public interface IRoleRepository:IGenericRepository<Role>
{
    Task<bool> HasActiveUsers(int roleId);
}