using Vneed.Data.Models;

namespace Vneed.Repositories.Interfaces;

public interface IDemandStatusHistoryRepository
{
    Task AddStatusHistory(DemandStatusHistory history);
}