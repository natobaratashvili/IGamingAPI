using Microsoft.AspNetCore.Mvc;
using IGaming.Core.UsersManagement.RequestModels;
using IGaming.Core.UsersManagement.Services.Interfaces;
using IGaming.Core.UsersManagement.ResponseModels;
using IGaming.API.Extensions;
using IGaming.API.CustomAttributes;
using IGaming.Core.Common;
namespace IGaming.API.Controllers
{
    [Route("api/v{version:apiVersion}/[controller]")]
    [ApiVersion("1.0")]
    [ApiController]
    public class UsersController : BaseController
    {
        private readonly IUserManagementService _userManagementService;
        public UsersController(IUserManagementService userManagementService)
        {
            _userManagementService = userManagementService;
        }
        [HttpPost]
        [Route("register")]
        [ValidateUserRegistration]
        [ProducesResponseType(type: typeof(Result), StatusCodes.Status201Created)]
        [ProducesResponseType(type: typeof(Result), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(type: typeof(Result), StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> Register([FromBody] UserRegistrationRequest userRegistration, CancellationToken cancellationToken)
        {
           var result = await _userManagementService.RegisterAsync(userRegistration, cancellationToken);
            return GenerateResponse(result, HttpContext);          
           
        }
        [HttpPost]
        [Route("authenticate")]
        [ProducesResponseType(type: typeof(Result<string>), StatusCodes.Status200OK)]
        [ProducesResponseType(type: typeof(Result), StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(type: typeof(Result), StatusCodes.Status500InternalServerError)]
        [AuthenticateRequestValidation]
        public async Task<IActionResult> Authenticate([FromBody] UserAuthenticateRequest userRegistration, CancellationToken cancellationToken)
        {
           var result = await _userManagementService.AuthenticateAsync(userRegistration, cancellationToken);
            return GenerateResponse(result, HttpContext);

        }

        [HttpGet]
        [Route("profile")]
        [CustomAuthorize]
        [ProducesResponseType(type: typeof(Result<UserProfileResponse>), StatusCodes.Status200OK)]
        [ProducesResponseType(type: typeof(Result), StatusCodes.Status404NotFound)]
        [ProducesResponseType(type: typeof(Result), StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(type: typeof(Result), StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> Profile(CancellationToken cancellationToken)
        {
            var res = await _userManagementService.GetProfileAsync(Request.GetCurrentUsername(), cancellationToken);
            return GenerateResponse(res, HttpContext);
        }

    }
}
