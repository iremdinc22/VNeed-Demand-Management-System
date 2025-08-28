using System.Linq.Expressions;
using System.Threading.Tasks;
using Vneed.Data.Models;
using Vneed.Services.Dto;

namespace Vneed.Services.Interfaces
{
    public interface IUserTokenService
    {
        Task<UserTokenDto> Create(UserTokenDto dto);
        Task<bool> Delete(int id);
        Task<UserTokenDto> GetByToken(string token);
        Task<UserTokenDto> GetByUserId(int userId);
        Task DeactivateTokensByUserId(int userId);
        Task<bool> Update(UserTokenDto dto);
        Task<IEnumerable<UserTokenDto>> GetAll(Expression<Func<UserToken, bool>> predicate);
    }
}