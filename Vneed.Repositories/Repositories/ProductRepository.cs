using Microsoft.EntityFrameworkCore;
using Vneed.Data.Context;
using Vneed.Data.Models;
using Vneed.Repositories.Interfaces;

namespace Vneed.Repositories.Repositories;

public class ProductRepository : GenericRepository<Product>, IProductRepository
{
    private readonly AppDbContext _context;

    public ProductRepository(AppDbContext context) : base(context)
    {
        _context = context;
    }

    public async Task<List<Product>> GetProductsByCategoryId(int categoryId)
    {
        return await _context.Product
            .Where(p => p.CategoryId == categoryId)
            .ToListAsync();
    }
    
    public async Task<Category> GetCategoryById(int categoryId)
    {
        return await _context.Category.FirstOrDefaultAsync(c => c.Id == categoryId);
    }
    
    public async Task<Product> GetByName(string name)
    {
        return await _context.Product
            .FirstOrDefaultAsync(p => p.Name.ToLower() == name.ToLower());
    }
    
    public async Task<bool> HasActiveDependencies(int productId)
    {
        // 1. Ürün hâlâ aktif mi?
        bool isActive = await _context.Product
            .AnyAsync(p => p.Id == productId && p.IsActive);

        // 2. Ürüne bağlı aktif (silinmemiş) talep var mı?
        bool hasDemands = await _context.Demand
            .AnyAsync(d => d.ProductId == productId); // Global query filter sayesinde DeletedAt == null filtreleniyor

        return isActive || hasDemands;
    }

    
}