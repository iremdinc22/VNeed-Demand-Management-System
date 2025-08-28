using Vneed.Services.Dto;
using Vneed.Common.Models;


namespace Vneed.Services.Interfaces;

public interface IRoleService
{
    Task<IEnumerable<RoleDto>> GetAll();
    Task<RoleDto> GetById(int id);
    Task<RoleDto> Add(RoleDto roleDto);
    Task<bool> Update(RoleDto roleDto);
    Task<bool> HasActiveUsers(int roleId);
    Task<bool> Delete(int roleId);
    
    //Paging için kullandık.
    Task<PagedResult<RoleDto>> GetPagedAsync(int pageNumber, int pageSize);
}