using System.Linq.Expressions;
using AutoMapper;
using System.Threading.Tasks;
using Vneed.Data.Models;
using Vneed.Repositories.Interfaces;
using Vneed.Services.Dto;
using Vneed.Services.Interfaces;

namespace Vneed.Services.Services
{
    public class UserTokenService : IUserTokenService
    {
        private readonly IUserTokenRepository _userTokenRepository;
        private readonly IMapper _mapper;

        public UserTokenService(IUserTokenRepository userTokenRepository, IMapper mapper)
        {
            _userTokenRepository = userTokenRepository;
            _mapper = mapper;
        }

        public async Task<UserTokenDto> Create(UserTokenDto dto)
        {
            var entity = _mapper.Map<UserToken>(dto);
            var created = await _userTokenRepository.Add(entity);
            return _mapper.Map<UserTokenDto>(created);
        }

        public async Task<bool> Delete(int id)
        {
            return await _userTokenRepository.Delete(id);
        }

        public async Task<UserTokenDto> GetByToken(string token)
        {
            var entity = await _userTokenRepository.GetByToken(token);
            return _mapper.Map<UserTokenDto>(entity);
        }

        public async Task<UserTokenDto> GetByUserId(int userId)
        {
            var entity = await _userTokenRepository.GetByUserId(userId);
            return _mapper.Map<UserTokenDto>(entity);
        }
        
        public async Task DeactivateTokensByUserId(int userId)
        {
            var tokens = await _userTokenRepository.GetAll(t => t.UserId == userId && t.IsActive);
            foreach (var token in tokens)
            {
                token.IsActive = false;
                await _userTokenRepository.Update(token);
            }
        }
        
        public async Task<bool> Update(UserTokenDto dto)
        {
            var entity = _mapper.Map<UserToken>(dto);
            return await _userTokenRepository.UpdateWithTracking(entity);
        }
        
        public async Task<IEnumerable<UserTokenDto>> GetAll(Expression<Func<UserToken, bool>> predicate)
        {
            var entities = await _userTokenRepository.GetAll(predicate);
            return _mapper.Map<IEnumerable<UserTokenDto>>(entities);
        }
    }
}