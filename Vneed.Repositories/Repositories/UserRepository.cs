using Microsoft.EntityFrameworkCore;
using Vneed.Data.Context;
using Vneed.Data.Models;
using Vneed.Repositories.Interfaces;

namespace Vneed.Repositories.Repositories;

public class UserRepository : GenericRepository<User>, IUserRepository
{
    private readonly AppDbContext _context;
    
    public UserRepository(AppDbContext context) : base(context)
    {
        _context = context;
    }
    
    public async Task<User> GetByEmail(string email)
    {
        return await _context.User.FirstOrDefaultAsync(u => u.Email == email);
    }
}