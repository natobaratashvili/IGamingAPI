using IGaming.API.Filters;
using IGaming.Core.Bets.RequestModels;
using Microsoft.AspNetCore.Mvc;

namespace IGaming.API.CustomAttributes
{
    /// <summary>
    /// Attribute for validating place bet requests using a <see cref="ValidationFilter{T}"/>.
    /// </summary>
    public class PlaceBetValidationAttribute : TypeFilterAttribute
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="PlaceBetValidationAttribute"/> class.
        /// </summary>
        /// <remarks>
        /// The constructor sets the type of the filter to <see cref="ValidationFilter{T}"/>, where T is <see cref="PlaceBetRequest"/>.
        /// </remarks>
        public PlaceBetValidationAttribute() : base(typeof(ValidationFilter<PlaceBetRequest>))
        {
        }
    }
}
