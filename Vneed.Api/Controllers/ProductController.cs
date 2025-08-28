using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Vneed.Services.Dto;
using Vneed.Services.Interfaces;
using Vneed.Data.Enum;
using Vneed.Common.Constants;
using Vneed.Common.Helpers;
using Vneed.Common.Models;

namespace Vneed.Api.Controllers
{
    [ApiController]
    [Route("api/product")]
    public class ProductController(IProductService productService,
        IProductSuggestionService productSuggestionService
    ) : ControllerBase
    
    {
        
        // Tüm ürünleri getir/
        [HttpGet]
        public async Task<IActionResult> GetAllProducts()
        {
            var result = await productService.GetAll();
    
            return Ok(ApiResponseHelper.SuccessWithData(new { product = result }, "Ürünler başarıyla getirildi."));
        }
        
      // Id'ye göre ürün getirir.
      [HttpGet("{id}")]
      [Authorize]
      public async Task<IActionResult> GetById(int id)
      {
        var product = await productService.GetById(id);
        if (product == null)
            return NotFound(ApiResponseHelper.Fail<object>("not_found", "Ürün bulunamadı."));

        return Ok(ApiResponseHelper.SuccessWithData(product, "Ürün başarıyla getirildi."));
      }
      
      // Sayfalama bilgilerine göre ürünleri listeler.
        
      [HttpGet("paged")]
      [Authorize]
      public async Task<IActionResult> GetPagedProduct(
          [FromQuery] int pageNumber = PaginationDefaults.DefaultPageNumber,
          [FromQuery] int pageSize = PaginationDefaults.DefaultPageSize)
      {
          try
          {
              var result = await productService.GetPagedAsync(pageNumber, pageSize);
              return Ok(ApiResponseHelper.SuccessWithData(result, "Ürünler başarıyla listelendi."));
          }
          catch (ArgumentException ex)
          {
              // Hata türüne göre kod belirleniyor.
              var errorCode = ex is ArgumentOutOfRangeException ? "out_of_range" : "invalid_paging";
              return BadRequest(ApiResponseHelper.Fail<object>(errorCode, ex.Message));
          }
      }
      
    //Ürün ekle
    [HttpPost]
    [Authorize(Roles = "3")]
    public async Task<IActionResult> Create([FromBody] ProductDto dto)
    {
        if (!ModelState.IsValid)
        {
            var validationErrors = ModelState
                .Where(e => e.Value.Errors.Count > 0)
                .Select(e => new ApiValidationErrorDetail
                {
                    Field = e.Key,
                    Messages = e.Value.Errors.Select(er => er.ErrorMessage).ToList()
                }).ToList();

            return BadRequest(ApiResponseHelper.ValidationFail(validationErrors));
        }

        var created = await productService.Create(dto);
        if (created == null)
            return BadRequest(ApiResponseHelper.Fail<object>("duplicate", "Bu ürün zaten mevcut."));

        return Ok(ApiResponseHelper.SuccessMessage("Ürün başarıyla oluşturuldu."));
    }

    //Ürün güncelleme
    [HttpPut("{id}")]
    [Authorize(Roles = "3")]
    public async Task<IActionResult> Update(int id, [FromBody] ProductDto dto)
    {
        dto.Id = id;
        var success = await productService.Update(dto);
        if (!success)
            return NotFound(ApiResponseHelper.Fail<object>("not_found", "Ürün bulunamadı."));

        return Ok(ApiResponseHelper.SuccessMessage("Ürün başarıyla güncellendi."));
    }

    [HttpDelete("{id}")]
    [Authorize(Roles = "3")]
    public async Task<IActionResult> Delete(int id)
    {
        var product = await productService.GetById(id);
        if (product == null)
            return NotFound(ApiResponseHelper.Fail<object>(
                "not_found", 
                "Ürün bulunamadı."));

        var hasDependencies = await productService.HasActiveDependencies(id);
        if (hasDependencies)
            return BadRequest(ApiResponseHelper.Fail<object>(
                "business", 
                "Bu ürüne ait aktif talepler bulunmaktadır. Ürünü silebilmek için öncelikle bu talepleri kaldırmanız gerekmektedir."));

        var success = await productService.Delete(id);
        if (!success)
            return StatusCode(500, ApiResponseHelper.Fail<object>(
                "delete_failed", 
                "Ürün silinirken bir hata oluştu."));

        return Ok(ApiResponseHelper.SuccessMessage("Ürün başarıyla silindi."));
    }
    
        [HttpGet("suggestion")]
        [Authorize(Roles = "1,2,3")]
        public async Task<IActionResult> GetSuggestions([FromQuery] bool onlyMine = false)
        {
            var userIdClaim = User.FindFirst("userId")?.Value;
            int? userId = string.IsNullOrEmpty(userIdClaim) ? null : int.Parse(userIdClaim);
            var roleClaim = User.FindFirst("role")?.Value;
            bool isAdmin = roleClaim == "3";
            if (onlyMine && userId != null && !isAdmin)
            {
                var suggestions = await productSuggestionService.GetUserSuggestions(userId.Value);
                return Ok(ApiResponseHelper.SuccessWithData(suggestions, "Kendi önerileriniz başarıyla getirildi."));
            }
            var allSuggestions = await productSuggestionService.GetAllSuggestions();
            return Ok(ApiResponseHelper.SuccessWithData(allSuggestions, "Öneriler başarıyla getirildi."));
        }
        
        // Id'ye göre ürün önerisi getirir.
        [HttpGet("suggestion/{id}")]
        public async Task<IActionResult> GetProductSuggestionById(int id)
        {
            var productSuggestion = await productSuggestionService.GetById(id);

            if (productSuggestion == null)
            {
                return NotFound(ApiResponseHelper.Fail<object>(
                    "not_found", 
                    "Product suggestion with id " + id + " not found."
                ));
            }
            return Ok(ApiResponseHelper.SuccessWithData(productSuggestion));
        }
        
        
        // Sayfalama bilgilerine göre ürünleri listeler.
        
        [HttpGet("suggestion/paged")]
        [Authorize]
        public async Task<IActionResult> GetPagedProductSuggestion(
            [FromQuery] int pageNumber = PaginationDefaults.DefaultPageNumber,
            [FromQuery] int pageSize = PaginationDefaults.DefaultPageSize)
        {
            try
            {
                var result = await productSuggestionService.GetPagedAsync(pageNumber, pageSize);
                return Ok(ApiResponseHelper.SuccessWithData(result, "Ürün önerileri başarıyla listelendi."));
            }
            catch (ArgumentException ex)
            {
                // Hata türüne göre kod belirleniyor.
                var errorCode = ex is ArgumentOutOfRangeException ? "out_of_range" : "invalid_paging";
                return BadRequest(ApiResponseHelper.Fail<object>(errorCode, ex.Message));
            }
        }
        
        
        // Ürün önerisi yap
        [HttpPost("suggestion")]
        [Authorize(Roles = "1,2")]
        public async Task<IActionResult> AddSuggestion([FromBody] ProductSuggestionDto dto)
        {
            var userIdClaim = User.FindFirst("userId")?.Value;
            if (string.IsNullOrEmpty(userIdClaim))
                return Unauthorized(ApiResponseHelper.Fail<object>("unauthorized", "Kullanıcı bulunamadı"));
            int userId = int.Parse(userIdClaim);

            try
            {
                await productSuggestionService.AddSuggestion(dto, userId);
                return Ok(ApiResponseHelper.SuccessMessage("Öneri başarıyla eklendi."));
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(ApiResponseHelper.Fail<object>("conflict", ex.Message));
            }
            catch (ArgumentException ex)
            {
                return BadRequest(ApiResponseHelper.Fail<object>("bad_request", ex.Message));
            }
        }
        
        // Ürün öneri onaylama reddetme
        [HttpPost("suggestion/{id}/action")]
        [Authorize(Roles = "3")]
        public async Task<IActionResult> HandleSuggestionAction(int id, [FromBody] SuggestionActionRequest request)
        {
            try
            {
                switch (request.Action)
                {
                    case SuggestionAction.Approve:
                        await productSuggestionService.ApproveSuggestion(id);
                        return Ok(ApiResponseHelper.SuccessMessage("Öneri onaylandı ve ürün oluşturuldu."));
                    case SuggestionAction.Reject:
                        await productSuggestionService.RejectSuggestion(id);
                        return Ok(ApiResponseHelper.SuccessMessage("Öneri reddedildi."));
                    default:
                        return BadRequest(ApiResponseHelper.Fail<object>("invalid_action", "Geçersiz işlem türü."));
                }
            }
            catch (KeyNotFoundException)
            {
                return NotFound(ApiResponseHelper.Fail<object>("not_found", "Öneri bulunamadı."));
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(ApiResponseHelper.Fail<object>("invalid_operation", ex.Message));
            }
            catch (Exception ex)
            {
                return BadRequest(ApiResponseHelper.Fail<object>("bad_request", ex.Message));
            }
        }


       // Kullanıcı kendi önerisini silebilir
        [HttpDelete("suggestion/{id}")]
        [Authorize(Roles = "1,2")]
        public async Task<IActionResult> DeleteSuggestion(int id)
        {
            var userIdClaim = User.FindFirst("userId")?.Value;
            if (string.IsNullOrEmpty(userIdClaim))
                return Unauthorized(ApiResponseHelper.Fail<object>("unauthorized", "Kullanıcı bilgisi alınamadı."));

            int userId = int.Parse(userIdClaim);

            try
            {
                var result = await productSuggestionService.DeleteSuggestion(id, userId);
                if (!result)
                    return NotFound(ApiResponseHelper.Fail<object>("not_found", "Öneri bulunamadı veya size ait değil."));

                return Ok(ApiResponseHelper.SuccessMessage("Öneri başarıyla silindi."));
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(ApiResponseHelper.Fail<object>("invalid_operation", ex.Message));
            }
        }


    }
}
