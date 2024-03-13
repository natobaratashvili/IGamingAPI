using IGaming.API.Filters;
using IGaming.Core.UsersManagement.RequestModels;
using Microsoft.AspNetCore.Mvc;

namespace IGaming.API.CustomAttributes
{
    public class ValidateUserRegistrationAttribute : TypeFilterAttribute
    {
        public ValidateUserRegistrationAttribute() : base(typeof(ValidationFilter<UserRegistrationRequest>))
        {
        }
    }
}
