using Vneed.Data.Models;

namespace Vneed.Repositories.Interfaces;

public interface ICategoryRepository:IGenericRepository<Category>
{
    Task<bool> HasActiveDependencies(int categoryId);
}

