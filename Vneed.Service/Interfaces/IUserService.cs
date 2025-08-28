using Vneed.Services.Dto;
using Vneed.Common.Models;

namespace Vneed.Services.Interfaces
{
    public interface IUserService
    {
        Task<List<UserDto>> GetAll();
        Task<UserDto> GetById(int id);
        Task<UserDto> Create(UserDto dto);
        Task<UserDto> Update(UserDto dto);
        Task<bool> Delete(int id);
        Task<UserDto> GetByEmail(string email);
        Task UpdateActiveStatus(UserActiveStatusDto dto);
        Task<List<UserDto>> GetActiveUsers();
        Task<List<UserDto>> GetInactiveUsers();
        Task<bool> UpdateByAdmin(int id, UserUpdateByAdminDto dto);
        Task<bool> UpdatePassword(int userId, string newPasswordHash);
        Task<bool> UpdatePasswordByEmail(string email, string newPasswordHash);
        Task<bool> ChangeUserRole(int userId, int newRoleId);
        Task<bool> CreateUserByAdmin(UserCreateByAdminDto dto);
       
        // Paging için ekledik
        Task<PagedResult<UserDto>> GetPagedAsync(int pageNumber, int pageSize);
    }
}