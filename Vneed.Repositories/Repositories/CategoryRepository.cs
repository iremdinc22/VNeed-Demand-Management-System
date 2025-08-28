using Vneed.Data.Context;
using Vneed.Data.Models;
using Vneed.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;


namespace Vneed.Repositories.Repositories;

public class CategoryRepository:GenericRepository<Category>, ICategoryRepository
{
    public CategoryRepository(AppDbContext context) : base(context)
    {
    }
    
    public async Task<bool> HasActiveDependencies(int categoryId)
    {
        bool hasProducts = await _context.Product
            .AnyAsync(p =>
                    p.CategoryId == categoryId &&
                    p.IsActive // sadece aktif ürünler (silinmemiş olanlar zaten filtreleniyor)
            );

        bool hasSuggestions = await _context.ProductSuggestions
            .AnyAsync(ps => ps.CategoryId == categoryId); // DeletedAt filtreleniyor otomatik

        return hasProducts || hasSuggestions;
    }


}