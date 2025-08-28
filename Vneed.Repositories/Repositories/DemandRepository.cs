using Microsoft.EntityFrameworkCore;
using Vneed.Data.Context;
using Vneed.Data.Enum;
using Vneed.Data.Models;
using Vneed.Repositories.Interfaces;
namespace Vneed.Repositories.Repositories;
public class DemandRepository : GenericRepository<Demand>, IDemandRepository
{
    private readonly AppDbContext _context;
    public DemandRepository(AppDbContext context) : base(context)
    {
        _context = context;
    }
    // Kullanıcıya ait talepleri listeleme
    public async Task<List<Demand>> GetDemandsByUserId(int userId)
    {
        return await _context.Demand
            .Where(d => d.UserId == userId)
            .ToListAsync();
    }
    // Statülere göre talepleri listeleme
    public async Task<List<Demand>> GetDemandsByStatus(DemandStatus status)
    {
        return await _context.Demand
            .Where(d => d.Status == status)
            .ToListAsync();
    }
    // Talepleri onaylama , reddetme , tammamlama
    public async Task UpdateDemandStatus(Guid demandId, DemandStatus newStatus)
    {
        var demand = await _context.Demand.FindAsync(demandId);
        if (demand != null)
        {
            demand.Status = newStatus;
            await _context.SaveChangesAsync();
        }
    }
    // Kullanıcılara ait talepleri statülerine göre getirme
    public async Task<List<Demand>> GetDemandsByUserAndStatus(int userId, DemandStatus status)
    {
        return await _context.Demand
            .Where(d => d.UserId == userId && d.Status == status)
            .ToListAsync();
    }
    // Kullanıcılara ait talepleri tarihe göre getirme
    public async Task<List<Demand>> GetDemandsByUserAndCreateDate(int userId, DateTime date)
    {
        return await _context.Demand
            .Where(d => d.UserId == userId && d.CreatedAt.Date == date.Date)
            .ToListAsync();
    }
    // Öncelik seviyesine göre olan talepleri getirme
    public async Task<List<Demand>> GetDemandsByPriority(PriorityLevel priority)
    {
        return await _context.Demand
            .Where(d => d.Priority == priority)
            .ToListAsync();
    }
    // En yeni talebi en üstte getirme sıralaması
    public async Task<List<Demand>> GetAllSortedByNewest()
    {
        return await _context.Demand
        .OrderByDescending(d => d.CreatedAt)
        .ToListAsync();
    }
    // En eski talebi en üstte getirme sıralaması
    public async Task<List<Demand>> GetAllSortedByOldest()
    {
        return await _context.Demand
        .OrderBy(d => d.CreatedAt)
        .ToListAsync();
    }
    // Belirli tarih aralığında talepleri getirme
    public async Task<List<Demand>> GetByDateRange(DateTime? startDate, DateTime? endDate)
    {
        var query = _context.Demand.AsQueryable();
        if (startDate.HasValue)
            query = query.Where(d => d.CreatedAt >= startDate.Value.Date);
        if (endDate.HasValue)
            query = query.Where(d => d.CreatedAt < endDate.Value.Date.AddDays(1));
        return await query.ToListAsync();
    }
    public async Task<List<Demand>> GetByIds(List<int> ids)
    {
        return await _context.Demand
            .Where(d => ids.Contains(d.Id))
            .ToListAsync();
    }
    
    public IQueryable<Demand> Query()
    {
        return _context.Demand.AsQueryable();
    }
    
    

}