using Vneed.Data.Enum;
using Vneed.Data.Models;
namespace Vneed.Repositories.Interfaces;
public interface IDemandRepository : IGenericRepository<Demand>
{
    Task<List<Demand>> GetDemandsByUserId(int userId);
    Task<List<Demand>> GetDemandsByStatus(DemandStatus status);
    Task UpdateDemandStatus(Guid demandId, DemandStatus newStatus);
    Task<List<Demand>> GetByIds(List<int> ids);
    Task<List<Demand>> GetDemandsByUserAndStatus(int userId, DemandStatus status);
    Task<List<Demand>> GetDemandsByUserAndCreateDate(int userId, DateTime date);
    Task<List<Demand>> GetDemandsByPriority(PriorityLevel Priority);
    Task<List<Demand>> GetAllSortedByNewest();
    Task<List<Demand>> GetAllSortedByOldest();
    Task<List<Demand>> GetByDateRange(DateTime? startDate, DateTime? endDate);
    IQueryable<Demand> Query();
    
}