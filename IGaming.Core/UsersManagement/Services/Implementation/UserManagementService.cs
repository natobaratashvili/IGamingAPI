using IGaming.Core.Common;
using IGaming.Core.Database;
using IGaming.Core.UsersManagement.Dtos;
using IGaming.Core.UsersManagement.Mappers;
using IGaming.Core.UsersManagement.Repositories.Interfaces;
using IGaming.Core.UsersManagement.RequestModels;
using IGaming.Core.UsersManagement.ResponseModels;
using IGaming.Core.UsersManagement.Security;
using IGaming.Core.UsersManagement.Services.Interfaces;

namespace IGaming.Core.UsersManagement.Services.Implementation
{
    /// <summary>
    /// Service responsible for managing user-related operations such as authentication, registration, and profile retrieval.
    /// </summary>
    public class UserManagementService : IUserManagementService
    {
        private readonly IUserRepository _userRepository;
        private readonly IHasher _hasher;
        private readonly IJwtProvider _jwtProvider;
        private readonly IDbWrapper _db;
        /// <summary>
        /// Initializes a new instance of the <see cref="UserManagementService"/> class.
        /// </summary>
        /// <param name="userRepository">The repository for user-related data.</param>
        /// <param name="hasher">The service responsible for hashing passwords.</param>
        /// <param name="jwtProvider">The provider for JWT tokens.</param>
        /// <param name="dbWrapper">The database wrapper for database operations.</param>
        public UserManagementService(IUserRepository userRepository, IHasher hasher, IJwtProvider jwtProvider, IDbWrapper dbWrapper)
        {
            _userRepository = userRepository;
            _hasher = hasher;
            _jwtProvider = jwtProvider;
            _db = dbWrapper;
        }
        /// <summary>
        /// Authenticates a user based on the provided credentials.
        /// </summary>
        /// <param name="authenticateRequest">The authentication request containing username and password.</param>
        /// <param name="cancellationToken">The token to monitor for cancellation requests.</param>
        /// <returns>A result indicating the success or failure of the authentication attempt.</returns>
        public async Task<Result> AuthenticateAsync(UserAuthenticateRequest authenticateRequest, CancellationToken cancellationToken)
        {
            var profile = await _db.RunAsync(async con => await _userRepository.GetByUserNameAsync(con, authenticateRequest.Username), cancellationToken);
            if (profile == null) 
                 return Result.Failure("Credentials", $"Invalid username or password.", 401);
            

            var isPasswordVerified = _hasher.Verify(authenticateRequest.Password,authenticateRequest.Username, profile?.HashedPassword);
            if (isPasswordVerified)
            {
                var calims = new Dictionary<string, string>
            {
                { "sub", profile.Guid },
                { "username", authenticateRequest.Username }
            };
                var token = _jwtProvider.GenerateToken(calims);
                return Result<string>.Success(token);
            }
            return Result.Failure("Credentials", $"Invalid username or password." , 401);
           
        }
        /// <summary>
        /// Retrieves the profile of a user with the specified username.
        /// </summary>
        /// <param name="username">The username of the user whose profile to retrieve.</param>
        /// <param name="cancellationToken">The token to monitor for cancellation requests.</param>
        /// <returns>A result containing the user's profile information if found; otherwise, a failure result.</returns>
        public async Task<Result> GetProfileAsync(string username, CancellationToken cancellationToken)
        {

            var profile =  await _db.RunAsync( async dbconnection =>  await _userRepository.GetByUserNameAsync(dbconnection, username), cancellationToken);
            if (profile == null) return Result.Failure("Username", $"User with username: {username} does not exist", 404);
            return Result<UserProfileResponse>.Success(profile.ToProfile());
        }

        /// <summary>
        /// Registers a new user with the provided registration information.
        /// </summary>
        /// <param name="userRegistration">The registration information for the new user.</param>
        /// <param name="cancellationToken">The token to monitor for cancellation requests.</param>
        /// <returns>A result indicating the success or failure of the registration attempt.</returns>
        public async Task<Result> RegisterAsync(UserRegistrationRequest userRegistration, CancellationToken cancellationToken)
        {
            var user = userRegistration.ToUserDto();
            user.HashedPassword = _hasher.Compute(userRegistration.Password, userRegistration.UserName);
            var result = await _db.RunWithTransactionAsync(async (dbconncetion, transaction) => 
            {
                var withSameEmail = await _userRepository.GetByEmailAsync(dbconncetion, userRegistration.Email, transaction);
                if (withSameEmail != null) return Result.Failure("Email", "Email is already in use.", 400);
                var withSameUserName = await _userRepository.GetByUserNameAsync(dbconncetion, userRegistration.UserName, transaction);
                if (withSameUserName != null) return Result.Failure("Username", "Username is already in use.", 400);
                 await _userRepository.CreateAsync(dbconncetion, user, transaction);
                return Result.Success(201);
            }, cancellationToken);

           
            return result;

        }
    }
}
