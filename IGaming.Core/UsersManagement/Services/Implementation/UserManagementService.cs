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
    public class UserManagementService : IUserManagementService
    {
        private readonly IUserRepository _userRepository;
        private readonly IHasher _hasher;
        private readonly IJwtProvider _jwtProvider;
        private readonly IDbWrapper _db;

        public UserManagementService(IUserRepository userRepository, IHasher hasher, IJwtProvider jwtProvider, IDbWrapper dbWrapper)
        {
            _userRepository = userRepository;
            _hasher = hasher;
            _jwtProvider = jwtProvider;
            _db = dbWrapper;
        }
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

        public async Task<Result> GetProfileAsync(string username, CancellationToken cancellationToken)
        {

            var profile =  await _db.RunAsync( async dbconnection =>  await _userRepository.GetByUserNameAsync(dbconnection, username), cancellationToken);
            if (profile == null) return Result.Failure("Username", $"User with username: {username} does not exist", 404);
            return Result<UserProfileResponse>.Success(profile.ToProfile());
        }

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
