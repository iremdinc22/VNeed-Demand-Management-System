using Vneed.Data.Context;
using Vneed.Data.Models;
using Vneed.Repositories.Interfaces;

namespace Vneed.Repositories.Repositories;

public class DemandStatusHistoryRepository:IDemandStatusHistoryRepository
{
    private readonly AppDbContext _context;

    public DemandStatusHistoryRepository(AppDbContext context)
    {
        _context = context;
    }

    public async Task AddStatusHistory(DemandStatusHistory history)
    {
        await _context.DemandStatusHistories.AddAsync(history);
        await _context.SaveChangesAsync();
    }
}