using IGaming.API.Filters;
using IGaming.Core.UsersManagement.RequestModels;
using Microsoft.AspNetCore.Mvc;

namespace IGaming.API.CustomAttributes
{
    /// <summary>
    /// Attribute for validating authentication requests using a <see cref="ValidationFilter{T}"/>.
    /// </summary>
    /// <remarks>
    /// This attribute is used to apply validation to authentication requests of type <typeparamref name="UserAuthenticateRequest"/>.
    /// It internally uses a <see cref="ValidationFilter{T}"/> to perform the validation logic.
    /// </remarks>
    public class AuthenticateRequestValidationAttribute : TypeFilterAttribute
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="AuthenticateRequestValidationAttribute"/> class.
        /// </summary>
        /// <remarks>
        /// The constructor sets the type of the filter to <see cref="ValidationFilter{T}"/>, where T is <see cref="UserAuthenticateRequest"/>.
        /// </remarks>
        public AuthenticateRequestValidationAttribute() : base(typeof(ValidationFilter<UserAuthenticateRequest>))
        {
        }
    }
}
