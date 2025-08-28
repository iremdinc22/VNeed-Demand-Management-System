using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Vneed.Common.Helpers;
using Vneed.Common.Models;
using Vneed.Services.Dto;
using Vneed.Services.Interfaces;
using Vneed.Common.Constants;
using Vneed.Data.Enum;

namespace Vneed.Api.Controllers;

[ApiController]
[Route("api/category")]
public class CategoryController(ICategoryService categoryService) : ControllerBase
{

    // Tüm kategorileri getir
    [HttpGet]
    [Authorize]
    public async Task<IActionResult> GetAll()
    {
        var categories = await categoryService.GetAll();
        return Ok(ApiResponseHelper.SuccessWithData(new { categories }, "Tüm kategoriler başarıyla getirildi."));
    }

    // Id'ye göre kategori getir
    [HttpGet("{id}")]
    [Authorize]
    public async Task<IActionResult> GetById(int id)
    {
        var category = await categoryService.GetById(id);
        if (category == null)
            return NotFound(ApiResponseHelper.Fail<object>("not_found", "Kategori bulunamadı."));

        return Ok(ApiResponseHelper.SuccessWithData(category, "Kategori başarıyla getirildi."));
    }
    
    
    // Sayfalama bilgilerine göre kategorileri listeler.
        
    [HttpGet("paged")]
    [Authorize]
    public async Task<IActionResult> GetPagedCategories(
        [FromQuery] int pageNumber = PaginationDefaults.DefaultPageNumber,
        [FromQuery] int pageSize = PaginationDefaults.DefaultPageSize)
    {
        try
        {
            var result = await categoryService.GetPagedAsync(pageNumber, pageSize);
            return Ok(ApiResponseHelper.SuccessWithData(result, "Kategoriler başarıyla listelendi."));
        }
        catch (ArgumentException ex)
        {
            // Hata türüne göre kod belirleniyor.
            var errorCode = ex is ArgumentOutOfRangeException ? "out_of_range" : "invalid_paging";
            return BadRequest(ApiResponseHelper.Fail<object>(errorCode, ex.Message));
        }
    }


    // Yeni kategori oluştur
    [HttpPost]
    [Authorize(Roles = "3")]
    public async Task<IActionResult> Create([FromBody] CategoryDto dto)
    {
        if (!ModelState.IsValid)
        {
            var errors = ModelState
                .Where(x => x.Value.Errors.Any())
                .Select(x => new ApiValidationErrorDetail
                {
                    Field = x.Key,
                    Messages = x.Value.Errors.Select(e => e.ErrorMessage).ToList()
                }).ToList();

            return BadRequest(ApiResponseHelper.ValidationFail(errors));
        }

        var created = await categoryService.Add(dto);
        return Ok(ApiResponseHelper.SuccessMessage("Kategori başarıyla oluşturuldu."));
    }

    // Kategori güncelle
    [HttpPut("{id}")]
    [Authorize(Roles = "3")]
    public async Task<IActionResult> Update(int id, [FromBody] CategoryDto dto)
    {
        dto.Id = id;
        
        var success = await categoryService.Update(dto);
        if (!success)
            return NotFound(ApiResponseHelper.Fail<object>("not_found", "Kategori bulunamadı."));

        return Ok(ApiResponseHelper.SuccessMessage("Kategori başarıyla güncellendi."));
    }

    // Kategori sil
    [HttpDelete("{id}")]
    [Authorize(Roles = "3")]
    public async Task<IActionResult> Delete(int id)
    {
        var result = await categoryService.Delete(id);

        return result switch
        {
            CategoryDeleteResult.NotFound => NotFound(ApiResponseHelper.Fail<object>(
                "not_found", "Kategori bulunamadı.")),

            CategoryDeleteResult.HasDependencies => BadRequest(ApiResponseHelper.Fail<object>(
                "business", "Kategori silinemedi. Lütfen önce bu kategoriye bağlı aktif ürünleri veya önerileri kaldırın.")),

            CategoryDeleteResult.Success => Ok(ApiResponseHelper.SuccessMessage("Kategori başarıyla silindi.")),

            _ => StatusCode(500, ApiResponseHelper.Fail<object>("unexpected", "Beklenmeyen bir hata oluştu."))
        };
    }

    
}
