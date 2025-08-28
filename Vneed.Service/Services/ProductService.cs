using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Vneed.Common.Helpers;
using Vneed.Common.Models;
using Vneed.Data.Enum;
using Vneed.Data.Models;
using Vneed.Repositories.Interfaces;
using Vneed.Repositories.Repositories;
using Vneed.Services.Dto;
using Vneed.Services.Interfaces;

public class ProductService : IProductService
{
    private readonly IProductRepository _productRepository;
    private readonly IMapper _mapper;

    public ProductService(IProductRepository productRepository, IMapper mapper)
    {
        _productRepository = productRepository;
        _mapper = mapper;
    }

    public async Task<List<ProductDto>> GetAll()
    {
        var entities = await _productRepository.GetAll();
        return _mapper.Map<List<ProductDto>>(entities);
    }

    public async Task<ProductDto?> GetById(int id)
    {
        var entity = await _productRepository.GetById(id);
        return entity == null ? null : _mapper.Map<ProductDto>(entity);
    }

    public async Task<ProductDto?> Create(ProductDto dto)
    {
        var existing = await _productRepository.GetWhere(p => p.Name.ToLower().Trim() == dto.Name.ToLower().Trim());
        if (existing.Any())
            return null;

        var entity = _mapper.Map<Product>(dto);
        var created = await _productRepository.Add(entity);
        return _mapper.Map<ProductDto>(created);
    }

    public async Task<bool> Update(ProductDto dto)
    {
        var existing = await _productRepository.GetById(dto.Id);
        if (existing == null)
            return false;

        await _productRepository.Update(_mapper.Map<Product>(dto));
        return true;
    }

    public async Task<bool> Delete(int id)
    {
        var exists = await _productRepository.GetById(id);
        if (exists == null)
            return false;

        await _productRepository.Delete(id);
        return true;
    }

    public async Task<bool> UpdateActiveStatus(ProductActiveStatusDto dto)
    {
        var product = await _productRepository.GetById(dto.Id);
        if (product == null)
            return false;

        product.IsActive = dto.IsActive;
        await _productRepository.Update(product);
        return true;
    }

    public async Task<List<ProductDto>> GetProductsByStatus(bool isActive)
    {
        var products = await _productRepository.GetWhere(x => x.IsActive == isActive);
        return _mapper.Map<List<ProductDto>>(products);
    } 
    
    
    // Paging methodu
    public async Task<PagedResult<ProductDto>> GetPagedAsync(int pageNumber, int pageSize)
    {
        var query = _productRepository.GetAllQueryable(); // IQueryable<>

        var totalCount = await query.CountAsync();

        PaginationValidator.Validate(pageNumber, pageSize, totalCount);

        var pagedData = await query
            .OrderByDescending(d => d.CreatedAt)
            .Skip((pageNumber - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync();

        var dtoList = _mapper.Map<List<ProductDto>>(pagedData);

        return new PagedResult<ProductDto>
        {
            Items = dtoList,
            Pagination = new PaginationInfo
            {
                PageNumber = pageNumber,
                PageSize = pageSize,
                TotalCount = totalCount,
                TotalPages = (int)Math.Ceiling(totalCount / (double)pageSize)
            }
        };
    }
    
    public async Task<bool> HasActiveDependencies(int productId)
    {
        return await _productRepository.HasActiveDependencies(productId);
    }

}
