using IGaming.Core.UsersManagement.Dtos;
using IGaming.Core.UsersManagement.RequestModels;
using System.Data;

namespace IGaming.Core.UsersManagement.Repositories.Interfaces
{
    public interface IUserRepository
    {
        Task CreateAsync(IDbConnection con, UserProfileDto user, IDbTransaction? transaction = null);
        Task<UserProfileDto> GetByEmailAsync(IDbConnection con, string email, IDbTransaction? transaction = null);
        Task<UserProfileDto> GetByUserNameAsync(IDbConnection con, string username, IDbTransaction? transaction = null);

    }
}
