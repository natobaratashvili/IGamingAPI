using IGaming.Core.UsersManagement.Dtos;
using IGaming.Core.UsersManagement.RequestModels;


namespace IGaming.Core.UsersManagement.Repositories.Interfaces
{
    public interface IUserRepository
    {
        Task CreateAsync(UserProfileDto user, CancellationToken cancellationToken);
        Task<UserProfileDto> GetAsync(string email, CancellationToken cancellationToken);

    }
}
