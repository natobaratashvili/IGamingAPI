using IGaming.Core.Common;
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
        Task<Result> RegisterAsync(UserRegistrationRequest userRegistration, CancellationToken cancellationToken);
        Task<Result> AuthenticateAsync(UserAuthenticateRequest authenticateRequest, CancellationToken cancellationToken);
        Task<Result> GetProfileAsync(string userName, CancellationToken cancellationToken);
    }
}
