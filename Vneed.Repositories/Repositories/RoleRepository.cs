using Vneed.Data.Context;
using Vneed.Data.Models;
using Vneed.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;
namespace Vneed.Repositories.Repositories;

public class RoleRepository:GenericRepository<Role>,IRoleRepository
{
    public RoleRepository(AppDbContext context) : base(context)
    {
        
    }
    
    public async Task<bool> HasActiveUsers(int roleId)
    {
        return await _context.User
            .AnyAsync(u => u.RoleId == roleId); 
        // Global query filter: DeletedAt == null otomatik çalışır
    }
}