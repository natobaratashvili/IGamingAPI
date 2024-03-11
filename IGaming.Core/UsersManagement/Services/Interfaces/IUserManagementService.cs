using IGaming.Core.UsersManagement.Dtos;
using IGaming.Core.UsersManagement.RequestModels;
using IGaming.Core.UsersManagement.ResponseModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IGaming.Core.UsersManagement.Services.Interfaces
{
    public interface IUserManagementService
    {
        Task RegisterAsync(UserRegistrationRequest userRegistration, CancellationToken cancellationToken);
        Task<string> AuthenticateAsync(UserAuthenticateRequest authenticateRequest, CancellationToken cancellationToken);
        Task<UserProfileResponse> GetProfileAsync(string userName, CancellationToken cancellationToken);
    }
}
