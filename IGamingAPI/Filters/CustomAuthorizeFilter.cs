using IGaming.Core.Common;
using IGaming.Core.UsersManagement.Security;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace IGaming.API.Filters
{
    /// <summary>
    /// Custom authorization filter for validating JWT tokens in incoming requests.
    /// </summary>
    public class CustomAuthorizeFilter : IAuthorizationFilter
    {
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IJwtProvider _jwtProvider;
        /// <summary>
        /// Initializes a new instance of the <see cref="CustomAuthorizeFilter"/> class.
        /// </summary>
        /// <param name="httpContextAccessor">The HTTP context accessor.</param>
        /// <param name="jwtProvider">The JWT provider for token validation.</param>
        public CustomAuthorizeFilter(IHttpContextAccessor httpContextAccessor, IJwtProvider jwtProvider)
        {
            _httpContextAccessor = httpContextAccessor;
            _jwtProvider = jwtProvider;
        }
        /// <summary>
        /// Performs token validation during authorization process.
        /// </summary>
        /// <param name="context">The authorization filter context.</param>
        public void OnAuthorization(AuthorizationFilterContext context)
        {
            var tokenWithBearer = _httpContextAccessor!.HttpContext!.Request.Headers["Authorization"].ToString();
            if (string.IsNullOrEmpty(tokenWithBearer))
            {
               HandleError(context);
                return;
            }
            var token = tokenWithBearer.Split(" ")[1];
            var validate = _jwtProvider.ValidateToken(token);
            if(!validate)
            {
               HandleError(context);
                return ;
            }
            return;
        }
        /// <summary>
        /// Handles authorization errors by setting the appropriate HTTP status code and response message.
        /// </summary>
        /// <param name="context">The authorization filter context.</param>
        public void HandleError(AuthorizationFilterContext context)
        {
            var response = Result.Failure("Authorization", "Authorization header is missing or invalid.", 401);
            context.HttpContext.Response.StatusCode = StatusCodes.Status401Unauthorized;
            context.Result = new ObjectResult(response);
        }

        
    }
}
