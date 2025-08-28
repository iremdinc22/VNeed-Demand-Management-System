using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Vneed.Data.Enum;
using Vneed.Data.Models;
using Vneed.Repositories.Interfaces;
using Vneed.Repositories.Repositories;
using Vneed.Services.Dto;
using Vneed.Services.Interfaces;
using Vneed.Common.Models;
using Vneed.Common.Helpers;


public class DemandService : IDemandService
{
    private readonly IDemandRepository _demandRepository;
    private readonly IDemandStatusHistoryRepository _demandStatusHistoryRepository;
    private readonly IUserRepository _userRepository;
    private readonly IMapper _mapper;


    public DemandService(
            IDemandRepository demandRepository,
            IDemandStatusHistoryRepository demandStatusHistoryRepository,
            IUserRepository userRepository,
            IMapper mapper)
    {
        _demandRepository = demandRepository;
        _demandStatusHistoryRepository = demandStatusHistoryRepository;
        _userRepository = userRepository;
        _mapper = mapper;
    }


    public async Task<List<DemandDto>> GetAll()
    {
        var entities = await _demandRepository.GetAll();
        return _mapper.Map<List<DemandDto>>(entities);
    }

    public async Task<DemandDto> GetById(int id)
    {
        var entity = await _demandRepository.GetById(id);
        return _mapper.Map<DemandDto>(entity);
    }

    public async Task<DemandDto> Create(DemandDto dto)
    {
        var entity = _mapper.Map<Demand>(dto);

        // Backend tarafında bu değerleri net bir şekilde set ediyoruz:
        entity.Status = DemandStatus.PendingTeamLeadApproval;
        entity.CreatedAt = DateTime.UtcNow;
        entity.UpdatedAt = DateTime.UtcNow;

        var created = await _demandRepository.Add(entity);
        return _mapper.Map<DemandDto>(created);
    }


    public async Task<DemandDto> Update(DemandDto dto)
    {
        var entity = _mapper.Map<Demand>(dto);
        var updated = await _demandRepository.Update(entity);
        return _mapper.Map<DemandDto>(updated);
    }

    public async Task<bool> Delete(int id)
    {
        return await _demandRepository.Delete(id);
    }

    public async Task<bool> BulkUpdateStatus(List<int> demandIds, int newStatus, int updatedByUserId)
    {
        var user = await _userRepository.GetById(updatedByUserId);
        if (user == null || user.Role == null) return false;

        var roleName = user.Role.Name; // "Admin", "TeamLead" gibi

        var targetStatus = (DemandStatus)newStatus;
        var now = DateTime.UtcNow;

        // :one: Rolün geçiş yapabileceği durumlar
        var allowedStatuses = roleName switch
        {
            "TeamLead" => new[] { DemandStatus.ApprovedByTeamLead, DemandStatus.RejectedByTeamLead },
            "Admin" => new[] { DemandStatus.Approved, DemandStatus.Rejected, DemandStatus.Completed },
            _ => Array.Empty<DemandStatus>()
        };

        if (!allowedStatuses.Contains(targetStatus))
            return false;

        // :two: Geçiş yapılabilmesi için mevcut durumda olması gereken statü
        var requiredCurrentStatus = targetStatus switch
        {
            DemandStatus.ApprovedByTeamLead => DemandStatus.PendingTeamLeadApproval,
            DemandStatus.RejectedByTeamLead => DemandStatus.PendingTeamLeadApproval,
            DemandStatus.Approved => DemandStatus.ApprovedByTeamLead,
            DemandStatus.Rejected => DemandStatus.ApprovedByTeamLead,
            DemandStatus.Completed => DemandStatus.Approved,
            _ => DemandStatus.PendingTeamLeadApproval
        };

        var demands = await _demandRepository.GetByIds(demandIds);
        if (demands == null || !demands.Any()) return false;

        foreach (var demand in demands)
        {
            if (demand.Status != requiredCurrentStatus)
                continue;

            demand.Status = targetStatus;
            demand.UpdatedAt = now;

            await _demandRepository.Update(demand);

            await _demandStatusHistoryRepository.AddStatusHistory(new DemandStatusHistory
            {
                DemandId = demand.Id,
                Status = targetStatus,
                ChangedAt = now,
                ChangedByUserId = updatedByUserId
            });
        }

        return true;
    }

    public async Task UpdateDemandStatus(Guid demandId, DemandStatus newStatus)
    {
        await _demandRepository.UpdateDemandStatus(demandId, newStatus);
    }

    public async Task<List<DemandDto>> FilterDemands(int? userId, DemandFilterDto filter)
    {
        var query = _demandRepository.Query();

        if (userId.HasValue)
            query = query.Where(d => d.UserId == userId.Value);

        if (filter.Status.HasValue)
            query = query.Where(d => d.Status == filter.Status);

        if (filter.Priority.HasValue)
            query = query.Where(d => d.Priority == filter.Priority);


        if (filter.StartDate.HasValue)
        {
            var startUtc = DateTime.SpecifyKind(filter.StartDate.Value, DateTimeKind.Utc);
            query = query.Where(d => d.CreatedAt >= startUtc);
        }

        if (filter.EndDate.HasValue)
        {
            var endUtc = DateTime.SpecifyKind(filter.EndDate.Value, DateTimeKind.Utc);
            var inclusiveEndDate = endUtc.Date.AddDays(1).AddTicks(-1); // 23:59:59.9999999
            query = query.Where(d => d.CreatedAt <= inclusiveEndDate);
        }

        query = filter.SortByNewest
            ? query.OrderByDescending(d => d.CreatedAt)
            : query.OrderBy(d => d.CreatedAt);

        var demands = await query.ToListAsync();
        return _mapper.Map<List<DemandDto>>(demands);
    }
    
    public async Task<bool> DeleteDemand(int demandId, int userId)
    {
        var demand = await _demandRepository.GetById(demandId);
        if (demand == null || demand.UserId != userId)
            return false;

        return await _demandRepository.Delete(demandId);
    }
    
    // Paging methodu
    public async Task<PagedResult<DemandDto>> GetPagedAsync(int pageNumber, int pageSize)
    {
        var query = _demandRepository.GetAllQueryable(); // IQueryable<>

        var totalCount = await query.CountAsync();

        PaginationValidator.Validate(pageNumber, pageSize, totalCount);

        var pagedData = await query
            .OrderByDescending(d => d.CreatedAt)
            .Skip((pageNumber - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync();

        var dtoList = _mapper.Map<List<DemandDto>>(pagedData);

        return new PagedResult<DemandDto>
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