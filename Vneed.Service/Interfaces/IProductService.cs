using Vneed.Common.Models;
using Vneed.Services.Dto;

namespace Vneed.Services.Interfaces
{
    public interface IProductService
    {
        Task<List<ProductDto>> GetAll();
        Task<ProductDto?> GetById(int id);
        Task<ProductDto?> Create(ProductDto dto);
        Task<bool> Update(ProductDto dto);
        Task<bool> Delete(int id);
        Task<bool> UpdateActiveStatus(ProductActiveStatusDto dto);
        Task<List<ProductDto>> GetProductsByStatus(bool isActive);
        // Paging methodu
        Task<PagedResult<ProductDto>> GetPagedAsync(int pageNumber, int pageSize);
        Task<bool> HasActiveDependencies(int productId);
    }

}