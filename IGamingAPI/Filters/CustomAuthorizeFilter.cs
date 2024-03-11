using IGaming.Core.UsersManagement.Security;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace IGaming.API.Filters
{
    public class CustomAuthorizeFilter : IAuthorizationFilter
    {
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IJwtProvider _jwtProvider;
        public CustomAuthorizeFilter(IHttpContextAccessor httpContextAccessor, IJwtProvider jwtProvider)
        {
            _httpContextAccessor = httpContextAccessor;
            _jwtProvider = jwtProvider;
        }
        public void OnAuthorization(AuthorizationFilterContext context)
        {
            var tokenWithBearer = _httpContextAccessor!.HttpContext!.Request.Headers["Authorization"].ToString();
            if (string.IsNullOrEmpty(tokenWithBearer))
            {
                context.Result = new UnauthorizedObjectResult(string.Empty);
                return;
            }
            var token = tokenWithBearer.Split(" ")[1];
            var validate = _jwtProvider.ValidateToken(token);
            if(!validate)
            {
                context.Result = new UnauthorizedObjectResult(string.Empty);
                return ;
            }
            return;
        }
    }
}
