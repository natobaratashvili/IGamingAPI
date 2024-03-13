using IGaming.API.Filters;
using IGaming.Core.UsersManagement.RequestModels;
using Microsoft.AspNetCore.Mvc;

namespace IGaming.API.CustomAttributes
{
    public class AuthenticateRequestValidationAttribute : TypeFilterAttribute
    {
        public AuthenticateRequestValidationAttribute() : base(typeof(ValidationFilter<UserAuthenticateRequest>))
        {
        }
    }
}
