using Vneed.Data.Models;

namespace Vneed.Repositories.Interfaces;

public interface IProductRepository:IGenericRepository<Product>
{
    Task<Category> GetCategoryById(int categoryId);
    Task<Product> GetByName(string name);
    Task<bool> HasActiveDependencies(int productId);
}