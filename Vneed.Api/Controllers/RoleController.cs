using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Vneed.Common.Helpers;
using Vneed.Common.Models;
using Vneed.Services.Dto;
using Vneed.Services.Interfaces;
using Vneed.Common.Constants;

namespace Vneed.Api.Controllers;

[ApiController]
[Route("api/role")]
[Authorize(Roles = "3")]
public class RoleController(IRoleService roleService) : ControllerBase
{

    // Tüm rolleri getirir.
    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        var roles = await roleService.GetAll();
        return Ok(ApiResponseHelper.SuccessWithData(roles, "Roller başarıyla getirildi."));
    }

    // Id'ye göre rol getirir.
    [HttpGet("{id}")]
    public async Task<IActionResult> GetById(int id)
    {
        var role = await roleService.GetById(id);
        if (role == null)
            return NotFound(ApiResponseHelper.Fail<object>("not_found", "Rol bulunamadı."));

        return Ok(ApiResponseHelper.SuccessWithData(role, "Rol başarıyla getirildi."));
    }
    
    
    // Sayfalama bilgilerine göre rolleri listeler.
        
    [HttpGet("paged")]
    [Authorize]
    public async Task<IActionResult> GetPagedRoles(
        [FromQuery] int pageNumber = PaginationDefaults.DefaultPageNumber,
        [FromQuery] int pageSize = PaginationDefaults.DefaultPageSize)
    {
        try
        {
            var result = await roleService.GetPagedAsync(pageNumber, pageSize);
            return Ok(ApiResponseHelper.SuccessWithData(result, "Roller başarıyla listelendi."));
        }
        catch (ArgumentException ex)
        {
            // Hata türüne göre kod belirleniyor.
            var errorCode = ex is ArgumentOutOfRangeException ? "out_of_range" : "invalid_paging";
            return BadRequest(ApiResponseHelper.Fail<object>(errorCode, ex.Message));
        }
    }

    // Rol oluşturur.
    [HttpPost]
    public async Task<IActionResult> Create([FromBody] RoleDto dto)
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

        var created = await roleService.Add(dto);
        return Ok(ApiResponseHelper.SuccessMessage("Rol başarıyla oluşturuldu."));
    }

    // Var olan rolü günceller
    [HttpPut("{id}")]
    public async Task<IActionResult> Update(int id, [FromBody] RoleDto dto)
    {
        dto.Id = id;
        var success = await roleService.Update(dto);
        if (!success)
            return NotFound(ApiResponseHelper.Fail<object>("not_found", "Güncellenecek rol bulunamadı."));

        return Ok(ApiResponseHelper.SuccessMessage("Rol başarıyla güncellendi."));
    }

    // Rol altındaki aktif kullanıcı yoksa rolü siler.
    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(int id)
    {
        var role = await roleService.GetById(id);
        if (role == null)
            return NotFound(ApiResponseHelper.Fail<object>(
                "not_found", 
                "Rol bulunamadı."));

        var hasUsers = await roleService.HasActiveUsers(id);
        if (hasUsers)
            return BadRequest(ApiResponseHelper.Fail<object>(
                "business", 
                "Bu role atanmış aktif kullanıcılar bulunmaktadır. Silme işlemi için lütfen bu kullanıcıları farklı bir role taşıyın veya sistemden kaldırın."));

        var success = await roleService.Delete(id);
        if (!success)
            return StatusCode(500, ApiResponseHelper.Fail<object>(
                "delete_failed", 
                "Rol silinirken bir hata oluştu."));

        return Ok(ApiResponseHelper.SuccessMessage("Rol başarıyla silindi."));
    }

}