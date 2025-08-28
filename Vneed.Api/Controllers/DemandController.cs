using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Vneed.Services.Dto;
using Vneed.Services.Interfaces;
using Vneed.Data.Enum;
using Vneed.Common.Helpers;
using Vneed.Common.Constants;

namespace Vneed.Api.Controllers
{
    [ApiController]
    [Route("api/demand")]
    public class DemandController(IDemandService demandService) : ControllerBase
    {
        
        private int? GetUserIdFromClaims()
        {
            var userIdClaim = User.FindFirst("userId")?.Value;
            return string.IsNullOrEmpty(userIdClaim) ? null : int.Parse(userIdClaim);
        }
        
        [HttpGet]
        [Authorize]
        public async Task<IActionResult> GetAll([FromQuery] DemandFilterDto? filter)
        {
            int? userId = null;

            // Eğer filtre yoksa, tüm talepleri döndür
            if (filter == null)
            {
                var all = await demandService.GetAll();
                return Ok(ApiResponseHelper.SuccessWithData(new { demands = all }, "Tüm talepler getirildi."));
            }

            // "Sadece kullanıcıya ait talepler" istenmişse, token'dan userId çek
            if (filter.OnlyCurrentUser)
            {
                userId = GetUserIdFromClaims();
                if (userId == null)
                {
                    return Unauthorized(ApiResponseHelper.Fail<object>(
                        "unauthorized",
                        "Kullanıcı kimliği doğrulanamadı. Lütfen giriş yaptığınızdan emin olun."));
                }
            }

            var result = await demandService.FilterDemands(userId, filter);
            return Ok(ApiResponseHelper.SuccessWithData(new { demand = result }, "Talepler başarıyla filtrelendi."));
        }
        
        
        // Id'ye göre talep getirir.
        [HttpGet("{id}")]
        [Authorize]
        public async Task<IActionResult> GetById(int id)
        {
            var result = await demandService.GetById(id);
            return result == null
                ? NotFound(ApiResponseHelper.Fail<object>("not_found", "Demand not found."))
                : Ok(ApiResponseHelper.SuccessWithData(result));
        }
        
        // Sayfalama bilgilerine göre talepleri listeler.
        
        [HttpGet("paged")]
        [Authorize]
        public async Task<IActionResult> GetPagedDemands(
            [FromQuery] int pageNumber = PaginationDefaults.DefaultPageNumber,
            [FromQuery] int pageSize = PaginationDefaults.DefaultPageSize)
        {
            try
            {
                var result = await demandService.GetPagedAsync(pageNumber, pageSize);
                return Ok(ApiResponseHelper.SuccessWithData(result, "Talepler başarıyla listelendi."));
            }
            catch (ArgumentException ex)
            {
                // Hata türüne göre kod belirle
                var errorCode = ex is ArgumentOutOfRangeException ? "out_of_range" : "invalid_paging";
                return BadRequest(ApiResponseHelper.Fail<object>(errorCode, ex.Message));
            }
        }

        // Talep oluşturur.
        [HttpPost]
        [Authorize]
        public async Task<IActionResult> Create([FromBody] DemandDto dto)
        {
            var userId = GetUserIdFromClaims();
            if (userId == null) return Unauthorized(ApiResponseHelper.Fail<object>("unauthorized", "User ID not found."));

            dto.UserId = userId.Value;
            var result = await demandService.Create(dto);
            return Ok(ApiResponseHelper.SuccessMessage("Demand created successfully."));
        }

        // Talep işlemleri
        [HttpPost("bulk-update-status")]
        [Authorize(Roles = "2,3")]
        public async Task<IActionResult> BulkUpdateStatus([FromBody] DemandBulkStatusUpdateRequest dto)
        {
            var role = User.Claims.FirstOrDefault(c => c.Type == "role")?.Value;
            var userId = GetUserIdFromClaims();
            if (userId == null)
                return Unauthorized(ApiResponseHelper.Fail<object>("unauthorized", "User ID not found."));

            var result = await demandService.BulkUpdateStatus(dto.DemandIds, dto.NewStatus, userId.Value);

            if (!result)
                return BadRequest(ApiResponseHelper.Fail<object>("update_failed", "İşlem yetkiniz yok veya durum güncellenemedi."));

            var message = ((DemandStatus)dto.NewStatus) switch
            {
                DemandStatus.ApprovedByTeamLead => "Talepler TeamLead tarafından onaylandı.",
                DemandStatus.RejectedByTeamLead => "Talepler TeamLead tarafından reddedildi.",
                DemandStatus.Approved => "Talepler Admin tarafından onaylandı.",
                DemandStatus.Rejected => "Talepler Admin tarafından reddedildi.",
                DemandStatus.Completed => "Talepler siparişe dönüştürüldü.",
                _ => "Taleplerin durumu güncellendi."
            };

            return Ok(ApiResponseHelper.SuccessMessage(message));
        }

        // Talep silme
        [HttpDelete("{id}")]
        [Authorize]
        public async Task<IActionResult> Delete(int id)
        {
            var userId = GetUserIdFromClaims();
            if (userId == null)
                return Unauthorized(ApiResponseHelper.Fail<object>("unauthorized", "User ID not found."));

            var result = await demandService.DeleteDemand(id, userId.Value);
            if (!result)
                return NotFound(ApiResponseHelper.Fail<object>("business", "Talep bulunamadı veya size ait değil."));

            return Ok(ApiResponseHelper.SuccessMessage("Talep başarıyla silindi."));
        }
        
        
        
    }
}