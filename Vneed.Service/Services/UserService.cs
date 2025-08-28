using AutoMapper;
using Vneed.Data.Models;
using Vneed.Repositories.Interfaces;
using Vneed.Services.Interfaces;
using Vneed.Services.Dto;
using Vneed.Common.Models;
using Microsoft.EntityFrameworkCore;
using Vneed.Common.Helpers;

namespace Vneed.Services.Services
{
    public class UserService : IUserService
    {
        private readonly IUserRepository _userRepository;
        private readonly IMapper _mapper;

        public UserService(IUserRepository userRepository, IMapper mapper)
        {
            _userRepository = userRepository;
            _mapper = mapper;
        }

        public async Task<List<UserDto>> GetAll()
        {
            var users = await _userRepository.GetAll();
            return _mapper.Map<List<UserDto>>(users);
        }

        public async Task<UserDto> GetById(int id)
        {
            var user = await _userRepository.GetById(id);
            return _mapper.Map<UserDto>(user);
        }

        public async Task<UserDto> Create(UserDto dto)
        {
            var user = new User
            {
                FullName = dto.FullName,
                Email = dto.Email,
                PasswordHash = dto.PasswordHash,
                RoleId = dto.RoleId,
                IsActive = dto.IsActive
            };

            var created = await _userRepository.Add(user);
            return _mapper.Map<UserDto>(created);
        }
        
        public async Task<UserDto> Update(UserDto dto)
        {
            var entity = _mapper.Map<User>(dto);
            var updated = await _userRepository.Update(entity);
            return _mapper.Map<UserDto>(updated);
        }

        public async Task<bool> Delete(int id)
        {
            return await _userRepository.Delete(id);
        }

        public async Task<UserDto> GetByEmail(string email)
        {
            var user = await _userRepository.GetByEmail(email);
            return _mapper.Map<UserDto>(user);
        }
        
        public async Task UpdateActiveStatus(UserActiveStatusDto dto)
        { 
            var user = await _userRepository.GetById(dto.Id);
            if (user == null)
                throw new Exception("Kullanıcı bulunamadı");
            user.IsActive = dto.IsActive;
            await _userRepository.Update(user);
        }
        
        public async Task<List<UserDto>> GetActiveUsers()
        {
            var activeUsers = await _userRepository.GetWhere(u => u.IsActive);
            return _mapper.Map<List<UserDto>>(activeUsers);
        }   
        
        public async Task<List<UserDto>> GetInactiveUsers()
        {
            var inactiveUsers = await _userRepository.GetWhere(u => u.IsActive==false);
            return _mapper.Map<List<UserDto>>(inactiveUsers);
        }
        
        public async Task<bool> UpdateByAdmin(int id, UserUpdateByAdminDto dto)
        {
            var user = await _userRepository.GetById(id);
            if (user == null)
                return false;

            user.FullName = dto.FullName;
            user.Email = dto.Email;
            user.RoleId = dto.RoleId;
            user.IsActive = dto.IsActive;

            await _userRepository.Update(user);
            return true;
        }

        
        public async Task<bool> UpdatePassword(int userId, string newPasswordHash)
        {
            var user = await _userRepository.GetById(userId);
            if (user == null)
                return false;

            user.PasswordHash = newPasswordHash;
            await _userRepository.Update(user);

            return true;
        }
        
        public async Task<bool> UpdatePasswordByEmail(string email, string newPasswordHash)
        {
            var user = await _userRepository.GetByEmail(email);
            if (user == null)
                return false;

            user.PasswordHash = newPasswordHash;
            await _userRepository.Update(user);

            return true;
        }
        
        public async Task<bool> ChangeUserRole(int userId, int newRoleId)
        {
            var user = await _userRepository.GetById(userId);
            if (user == null)
                return false;

            user.RoleId = newRoleId;
            await _userRepository.Update(user);
            return true;
        }

        public async Task<bool> CreateUserByAdmin(UserCreateByAdminDto dto)
        {
            var existingUser = await _userRepository.GetByEmail(dto.Email);
            if (existingUser != null)
            {
                return false;
            }

            var newUser = new User
            {
                Email = dto.Email,
                FullName = dto.FullName,
                RoleId = dto.RoleId,
                PasswordHash = null,
                IsActive = true
            };

            var createdUser = await _userRepository.Add(newUser);
            return createdUser != null;
        }
        
        
        // Paging methodu
        public async Task<PagedResult<UserDto>> GetPagedAsync(int pageNumber, int pageSize)
        {
            var query = _userRepository.GetAllQueryable(); // IQueryable<>

            var totalCount = await query.CountAsync();

            PaginationValidator.Validate(pageNumber, pageSize, totalCount);

            var pagedData = await query
                .OrderByDescending(d => d.CreatedAt)
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

            var dtoList = _mapper.Map<List<UserDto>>(pagedData);

            return new PagedResult<UserDto>
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
}