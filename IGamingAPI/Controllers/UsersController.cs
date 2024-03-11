using Microsoft.AspNetCore.Mvc;
using IGaming.Core.UsersManagement.RequestModels;
using IGaming.Core.UsersManagement.Services.Interfaces;
using IGaming.Core.UsersManagement.Dtos;
using IGaming.Core.UsersManagement.ResponseModels;
using IGaming.API.Filters;
using IGaming.API.Extensions;
namespace IGaming.API.Controllers
{
    [Route("api/v{version:apiVersion}/[controller]")]
    [ApiVersion("1.0")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private readonly IUserManagementService _userManagementService;
        public UsersController(IUserManagementService userManagementService)
        {
            _userManagementService = userManagementService;
        }
        [HttpPost]
        [Route("register")]
        public async Task<IActionResult> Register([FromBody] UserRegistrationRequest userRegistration, CancellationToken cancellationToken)
        {
           await _userManagementService.RegisterAsync(userRegistration, cancellationToken);
           return Ok();
           
        }
        [HttpPost]
        [Route("authenticate")]
        public async Task<IActionResult> Authenticate([FromBody] UserAuthenticateRequest userRegistration, CancellationToken cancellationToken)
        {
           var token = await _userManagementService.AuthenticateAsync(userRegistration, cancellationToken);
            return Ok(token);

        }

        [HttpGet]
        [Route("profile")]
        [CustomAuthorize]
        public async Task<ActionResult<UserProfileResponse>> Profile(CancellationToken cancellationToken)
        {
            var res = await _userManagementService.GetProfileAsync(HttpContext.Request.GetCurrentUsername(), cancellationToken);
            return Ok(res);
        }

    }
}
