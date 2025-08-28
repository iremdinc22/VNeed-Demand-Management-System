using Vneed.Data.Enum;
using Vneed.Services.Dto;
using Vneed.Common.Models;
namespace Vneed.Services.Interfaces
{
    public interface IDemandService
    {
        // Ortak işlemler
        Task<List<DemandDto>> GetAll();
        Task<DemandDto> GetById(int id);
        Task<DemandDto> Create(DemandDto dto);
        Task<DemandDto> Update(DemandDto dto);
        Task<bool> Delete(int id);
        Task<bool> BulkUpdateStatus(List<int> demandIds, int newStatus, int updatedByUserId);
        // Filtrelemeler
        Task<List<DemandDto>> FilterDemands(int? userId, DemandFilterDto filter);
        Task UpdateDemandStatus(Guid demandId, DemandStatus newStatus);
        Task<bool> DeleteDemand(int demandId, int userId);
        // Paging methodu
        Task<PagedResult<DemandDto>> GetPagedAsync(int pageNumber, int pageSize);
    }
}