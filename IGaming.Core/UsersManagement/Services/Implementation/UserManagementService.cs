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

        public UserManagementService(IUserRepository userRepository, IHasher hasher, IJwtProvider jwtProvider)
        {
            _userRepository = userRepository;
            _hasher = hasher;
            _jwtProvider = jwtProvider;
        }
        public async Task<string> AuthenticateAsync(UserAuthenticateRequest authenticateRequest, CancellationToken cancellationToken)
        {
            //check fot null and not verified
            var profile = await _userRepository.GetAsync(authenticateRequest.Username, cancellationToken);
            var isPasswordVerified = _hasher.Verify(authenticateRequest.Password, profile?.HashedPassword);
            if (isPasswordVerified)
            {
                var calims = new Dictionary<string, string>
            {
                { "sub", profile.Guid },
                { "username", authenticateRequest.Username }
            };
                var token = _jwtProvider.GenerateToken(calims);
                return token;
            }
            return "ffrfr";
           
        }

        public async Task<UserProfileResponse> GetProfileAsync(string userName, CancellationToken cancellationToken)
        {
            var profile =  await _userRepository.GetAsync(userName, cancellationToken);
            return profile.ToProfile();
        }

        public async Task RegisterAsync(UserRegistrationRequest userRegistration, CancellationToken cancellationToken)
        {
            var user = userRegistration.ToUserDto();
            user.HashedPassword = _hasher.Compute(userRegistration.Password);
            await _userRepository.CreateAsync(user, cancellationToken);

        }
    }
}
