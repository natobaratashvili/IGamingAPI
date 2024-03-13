using IGaming.Core.Common;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Mysqlx;

namespace IGaming.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BaseController : ControllerBase
    {
        public static IActionResult GenerateResponse(Result result, HttpContext context)
        {
      
            var code = result.GetStatusCode();
            return code switch
            {
                200 => new OkObjectResult(result),
                201 => new CreatedResult(context.Request.Path.ToString(), result),
                204 => new NoContentResult(),
                400 => new BadRequestObjectResult(result),
                401 => new UnauthorizedObjectResult(result),
                404 => new NotFoundObjectResult(result),
                _ => new OkObjectResult(result),
            };
        }
    }
}
