using AutoMapper;
using Vneed.Data.Models;
using Vneed.Repositories.Interfaces;
using Vneed.Services.Dto;
using Vneed.Services.Interfaces;
using Microsoft.EntityFrameworkCore;
using Vneed.Common.Helpers;
using Vneed.Common.Models;

namespace Vneed.Services.Services;

public class ProductSuggestionService : IProductSuggestionService
    {
        private readonly IProductSuggestionRepository _suggestionRepository;
        private readonly IProductRepository _productRepository;
        private readonly IMapper _mapper;

        public ProductSuggestionService(
            IProductSuggestionRepository suggestionRepository,
            IProductRepository productRepository,
            IMapper mapper)
        {
            _suggestionRepository = suggestionRepository;
            _productRepository = productRepository;
            _mapper = mapper;
        }

        public async Task AddSuggestion(ProductSuggestionDto dto, int userId)
        {
            var category = await _productRepository.GetCategoryById(dto.CategoryId);
            if (category == null)
                throw new ArgumentException("Geçersiz kategori seçimi.");

            var existingProduct = await _productRepository.GetByName(dto.SuggestedName);
            if (existingProduct != null)
                throw new InvalidOperationException("Bu ürün zaten mevcut.");

            var existingSuggestion = await _suggestionRepository.GetByUserIdAndName(userId, dto.SuggestedName);
            if (existingSuggestion != null && existingSuggestion.IsApproved != false)
                throw new InvalidOperationException("Bu ürünü zaten önerdiniz ve işlem bekliyor.");

            var entity = _mapper.Map<ProductSuggestion>(dto);

            entity.UserId = userId;
            entity.CreatedAt = DateTime.UtcNow;
            entity.IsApproved = null;
            entity.Category = null;
            entity.CategoryId = dto.CategoryId;

            await _suggestionRepository.Add(entity);
        }



        public async Task<IEnumerable<ProductSuggestionDto>> GetUserSuggestions(int userId)
        {
            var suggestions = await _suggestionRepository.GetByUserId(userId);
            return _mapper.Map<IEnumerable<ProductSuggestionDto>>(suggestions);
        }

        public async Task<IEnumerable<ProductSuggestionDto>> GetAllSuggestions()
        {
            var suggestions = await _suggestionRepository.GetAll();
            return _mapper.Map<IEnumerable<ProductSuggestionDto>>(suggestions);
        }

        public async Task ApproveSuggestion(int suggestionId)
        {
            var suggestion = await _suggestionRepository.GetById(suggestionId);
            if (suggestion == null)
                throw new KeyNotFoundException("Öneri bulunamadı.");

            if (suggestion.IsApproved == false)
                throw new InvalidOperationException("Bu öneri zaten reddedildi ve onaylanamaz.");

            if (suggestion.IsApproved == true)
                throw new InvalidOperationException("Bu öneri zaten onaylandı.");

            suggestion.IsApproved = true;
            await _suggestionRepository.Update(suggestion);

            var newProduct = new Product
            {
                Name = suggestion.SuggestedName,
                CategoryId = suggestion.CategoryId,
                CreatedAt = DateTime.UtcNow,
                IsActive = true
            };
            await _productRepository.Add(newProduct);
        }

        public async Task RejectSuggestion(int suggestionId)
        {
            var suggestion = await _suggestionRepository.GetById(suggestionId);
            if (suggestion == null)
                throw new KeyNotFoundException("Öneri bulunamadı.");

            if (suggestion.IsApproved == true)
                throw new InvalidOperationException("Bu öneri zaten onaylandı ve reddedilemez.");

            if (suggestion.IsApproved == false)
                throw new InvalidOperationException("Bu öneri zaten reddedildi.");

            suggestion.IsApproved = false;
            await _suggestionRepository.Update(suggestion);
        }
        
        public async Task<ProductSuggestionDto> GetById(int id)
        {
            var productSuggestion = await _suggestionRepository.GetById(id);

            if (productSuggestion == null)
                return null;

            return _mapper.Map<ProductSuggestionDto>(productSuggestion);
        }
        
        public async Task<bool> DeleteSuggestion(int suggestionId, int userId)
        {
            var suggestion = await _suggestionRepository.GetById(suggestionId);
            if (suggestion == null)
                return false;

            if (suggestion.UserId != userId)
                return false;

            if (suggestion.IsApproved != null)
                throw new InvalidOperationException("Onaylanmış veya reddedilmiş bir öneri silinemez.");

            await _suggestionRepository.Delete(suggestion.Id); 
            return true;
        }
        
        
        // Paging methodu
        public async Task<PagedResult<ProductSuggestionDto>> GetPagedAsync(int pageNumber, int pageSize)
        {
            var query = _suggestionRepository.GetAllQueryable(); // IQueryable<>

            var totalCount = await query.CountAsync();

            PaginationValidator.Validate(pageNumber, pageSize, totalCount);

             //var pagedData = await query
               // .OrderByDescending(d => d.CreatedAt)
              //  .Skip((pageNumber - 1) * pageSize)
               // .Take(pageSize)
              //  .ToListAsync();
             
              var pagedData = await query
                  .AsNoTracking()
                  .Include(x => x.Category) 
                  .OrderByDescending(d => d.CreatedAt)
                  .Skip((pageNumber - 1) * pageSize)
                  .Take(pageSize)
                  .ToListAsync();

              

            var dtoList = _mapper.Map<List<ProductSuggestionDto>>(pagedData);

            return new PagedResult<ProductSuggestionDto>
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