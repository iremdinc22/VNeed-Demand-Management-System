using Vneed.Data.Models;

namespace Vneed.Repositories.Interfaces;

public interface IProductSuggestionRepository : IGenericRepository<ProductSuggestion>
{
    Task<IEnumerable<ProductSuggestion>> GetByUserId(int userId);
    Task<ProductSuggestion?> GetByUserIdAndName(int userId, string suggestedName); 
}