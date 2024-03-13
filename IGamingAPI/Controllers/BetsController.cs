using IGaming.API.CustomAttributes;
using IGaming.API.Extensions;
using IGaming.Core.Bets.RequestModels;
using IGaming.Core.Bets.Services.Interfaces;
using IGaming.Core.Common;
using IGaming.Core.UsersManagement.RequestModels;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace IGaming.API.Controllers
{
    [Route("api/v{version:apiVersion}/[controller]")]
    [ApiVersion("1.0")]
    [ApiController]
    public class BetsController : BaseController
    {
        private readonly IBetsManagementService _betsManagementService;
        public BetsController(IBetsManagementService betsManagementService)
        {
           _betsManagementService = betsManagementService;
        }
        [HttpPost]
        [Route("place")]
        [PlaceBetValidation]
        [CustomAuthorize]
        [ProducesResponseType(type: typeof(Result), StatusCodes.Status204NoContent)]
        [ProducesResponseType(type: typeof(Result), StatusCodes.Status500InternalServerError)]
        [ProducesResponseType(type: typeof(Result), StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(type: typeof(Result), StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> Place([FromBody] PlaceBetRequest placeBetRequest, CancellationToken cancellationToken)
        {
           var result = await _betsManagementService.PlaceAsync(placeBetRequest, HttpContext.Request.GetCurrentUsername(), cancellationToken);
            return GenerateResponse(result, HttpContext);


        }
    }
}
