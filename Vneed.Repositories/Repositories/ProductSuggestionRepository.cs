using Microsoft.EntityFrameworkCore;
using Vneed.Data.Context;
using Vneed.Data.Models;
using Vneed.Repositories.Interfaces;

namespace Vneed.Repositories.Repositories;

public class ProductSuggestionRepository : GenericRepository<ProductSuggestion>, IProductSuggestionRepository
{
    private readonly AppDbContext _context;

    public ProductSuggestionRepository(AppDbContext context) : base(context)
    {
        _context = context;
    }

    public async Task<IEnumerable<ProductSuggestion>> GetByUserId(int userId)
    {
        return await _context.ProductSuggestions
            .Where(p => p.UserId == userId)
            .Include(p => p.Category)
            .ToListAsync();
    }

    public async Task<ProductSuggestion?> GetByIdWithCategory(int id)
    {
        return await _context.ProductSuggestions
            .Include(p => p.Category)
            .FirstOrDefaultAsync(p => p.Id == id);
    }
    public async Task<ProductSuggestion?> GetByUserIdAndName(int userId, string suggestedName)
    {
        return await _context.ProductSuggestions
            .Where(p => p.UserId == userId && p.SuggestedName.ToLower() == suggestedName.ToLower())
            .FirstOrDefaultAsync();
    }
}