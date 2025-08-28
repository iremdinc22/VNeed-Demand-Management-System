using Vneed.Services.Dto;
using Vneed.Common.Models;


namespace Vneed.Services.Interfaces;

public interface IProductSuggestionService
{
    Task AddSuggestion(ProductSuggestionDto dto, int userId);
    Task<IEnumerable<ProductSuggestionDto>> GetUserSuggestions(int userId);
    Task<IEnumerable<ProductSuggestionDto>> GetAllSuggestions();
    Task ApproveSuggestion(int suggestionId);
    Task RejectSuggestion(int suggestionId);
    Task<ProductSuggestionDto> GetById(int id);
    Task<bool> DeleteSuggestion(int suggestionId, int userId);
    // Paging methodunu burda kullandık.
    Task<PagedResult<ProductSuggestionDto>> GetPagedAsync(int pageNumber, int pageSize);


}