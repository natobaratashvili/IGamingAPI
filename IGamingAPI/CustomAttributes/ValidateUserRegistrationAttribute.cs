using IGaming.API.Filters;
using IGaming.Core.UsersManagement.RequestModels;
using Microsoft.AspNetCore.Mvc;

namespace IGaming.API.CustomAttributes
{
    /// <summary>
    /// Attribute for validating user registration requests using a <see cref="ValidationFilter{T}"/>.
    /// </summary>
    public class ValidateUserRegistrationAttribute : TypeFilterAttribute
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ValidateUserRegistrationAttribute"/> class.
        /// </summary>
        /// <remarks>
        /// The constructor sets the type of the filter to <see cref="ValidationFilter{T}"/>, where T is <see cref="UserRegistrationRequest"/>.
        /// </remarks>
        public ValidateUserRegistrationAttribute() : base(typeof(ValidationFilter<UserRegistrationRequest>))
        {
        }
    }
}
