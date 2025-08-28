using AutoMapper;
using Vneed.Data.Models;
using Vneed.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;
using Vneed.Services.Dto;
using Vneed.Services.Interfaces;
using Vneed.Common.Models;
using Vneed.Common.Helpers;

namespace Vneed.Services.Services;

public class RoleService : IRoleService
{
    private readonly IRoleRepository _roleRepository;
    private readonly IMapper _mapper;

    public RoleService(IRoleRepository roleRepository, IMapper mapper)
    {
        _roleRepository = roleRepository;
        _mapper = mapper;
    }

    public async Task<IEnumerable<RoleDto>> GetAll()
    {
        var roles = await _roleRepository.GetAll();
        return _mapper.Map<IEnumerable<RoleDto>>(roles);
    }

    public async Task<RoleDto> GetById(int id)
    {
        var role = await _roleRepository.GetById(id);
        return _mapper.Map<RoleDto>(role);
    }

    public async Task<RoleDto> Add(RoleDto roleDto)
    {
        var role = _mapper.Map<Role>(roleDto);
        var added = await _roleRepository.Add(role);
        return _mapper.Map<RoleDto>(added);
    }

    public async Task<bool> Update(RoleDto roleDto)
    {
        var role = _mapper.Map<Role>(roleDto);
        var updated = await _roleRepository.Update(role);
        return updated != null;
    }

    public async Task<bool> HasActiveUsers(int roleId)
    {
        return await _roleRepository.HasActiveUsers(roleId);
    }

    public async Task<bool> Delete(int roleId)
    {
        var role = await _roleRepository.GetById(roleId);
        if (role == null)
            return false;

        var hasUsers = await _roleRepository.HasActiveUsers(roleId);
        if (hasUsers)
            return false;

        role.DeletedAt = DateTimeOffset.UtcNow;
        await _roleRepository.Update(role);
        return true;
    }
    
    
    // Paging methodu
    public async Task<PagedResult<RoleDto>> GetPagedAsync(int pageNumber, int pageSize)
    {
        var query = _roleRepository.GetAllQueryable(); // IQueryable<>

        var totalCount = await query.CountAsync();

        PaginationValidator.Validate(pageNumber, pageSize, totalCount);

        var pagedData = await query
            .OrderByDescending(d => d.Id) 
            .Skip((pageNumber - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync();


        var dtoList = _mapper.Map<List<RoleDto>>(pagedData);

        return new PagedResult<RoleDto>
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
