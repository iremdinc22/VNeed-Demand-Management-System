using Vneed.Common.Models;
using Vneed.Services.Dto;
using Vneed.Data.Enum;

namespace Vneed.Services.Interfaces;

public interface ICategoryService
{
    Task<IEnumerable<CategoryDto>> GetAll();
    Task<CategoryDto?> GetById(int id);
    Task<CategoryDto> Add(CategoryDto dto);
    Task<bool> Update(CategoryDto dto);
    Task<CategoryDeleteResult> Delete(int categoryId);
    Task<PagedResult<CategoryDto>> GetPagedAsync(int pageNumber, int pageSize);
}