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
    /// <summary>
    /// Interface for user management operations.
    /// </summary>
    public interface IUserManagementService
    {
        /// <summary>
        /// Registers a new user asynchronously.
        /// </summary>
        /// <param name="userRegistration">The user registration request.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns>A task representing the asynchronous operation. The result of the registration attempt.</returns>
        Task<Result> RegisterAsync(UserRegistrationRequest userRegistration, CancellationToken cancellationToken);
        /// <summary>
        /// Authenticates a user asynchronously.
        /// </summary>
        /// <param name="authenticateRequest">The authentication request.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns>A task representing the asynchronous operation. The result of the authentication attempt.</returns>
        Task<Result> AuthenticateAsync(UserAuthenticateRequest authenticateRequest, CancellationToken cancellationToken);
        /// <summary>
        /// Retrieves the profile of a user asynchronously.
        /// </summary>
        /// <param name="userName">The username of the user.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns>A task representing the asynchronous operation. The result containing the user profile.</returns>
        Task<Result> GetProfileAsync(string userName, CancellationToken cancellationToken);
    }
}
