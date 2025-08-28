using AutoMapper;
using Vneed.Data.Models;
using Vneed.Repositories.Interfaces;
using Vneed.Services.Dto;
using Vneed.Services.Interfaces;
using Vneed.Common.Helpers;
using Vneed.Common.Models;
using Microsoft.EntityFrameworkCore;
using Vneed.Data.Enum;



namespace Vneed.Services.Services;

public class CategoryService : ICategoryService
{
    private readonly ICategoryRepository _categoryRepository;
    private readonly IMapper _mapper;

    public CategoryService(ICategoryRepository categoryRepository, IMapper mapper)
    {
        _categoryRepository = categoryRepository;
        _mapper = mapper;
    }

    public async Task<IEnumerable<CategoryDto>> GetAll()
    {
        var categories = await _categoryRepository.GetAll();
        return _mapper.Map<IEnumerable<CategoryDto>>(categories);
    }

    public async Task<CategoryDto?> GetById(int id)
    {
        var category = await _categoryRepository.GetById(id);
        return category == null ? null : _mapper.Map<CategoryDto>(category);
    }

    public async Task<CategoryDto> Add(CategoryDto dto)
    {
        var entity = _mapper.Map<Category>(dto);
        var created = await _categoryRepository.Add(entity);
        return _mapper.Map<CategoryDto>(created);
    }

    public async Task<bool> Update(CategoryDto dto)
    {
        var category = _mapper.Map<Category>(dto);
        var updated = await _categoryRepository.Update(category);
        return updated != null;
    }
    
    public async Task<CategoryDeleteResult> Delete(int categoryId)
    {
        var category = await _categoryRepository.GetById(categoryId);
        if (category == null)
            return CategoryDeleteResult.NotFound;

        bool hasDependencies = await _categoryRepository.HasActiveDependencies(categoryId);
        if (hasDependencies)
            return CategoryDeleteResult.HasDependencies;

        category.DeletedAt = DateTimeOffset.UtcNow;
        await _categoryRepository.Update(category);
        return CategoryDeleteResult.Success;
    }

    
    
    // Paging methodu
    public async Task<PagedResult<CategoryDto>> GetPagedAsync(int pageNumber, int pageSize)
    {
        var query = _categoryRepository.GetAllQueryable(); // IQueryable<>

        var totalCount = await query.CountAsync();

        PaginationValidator.Validate(pageNumber, pageSize, totalCount);

        var pagedData = await query
            .OrderByDescending(d => d.CreatedAt)
            .Skip((pageNumber - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync();

        var dtoList = _mapper.Map<List<CategoryDto>>(pagedData);

        return new PagedResult<CategoryDto>
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
    
    
}