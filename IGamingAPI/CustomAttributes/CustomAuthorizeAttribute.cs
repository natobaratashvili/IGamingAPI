using IGaming.API.Filters;
using Microsoft.AspNetCore.Mvc;

namespace IGaming.API.CustomAttributes
{
    /// <summary>
    /// Attribute for custom authorization using a <see cref="CustomAuthorizeFilter"/>.
    /// </summary>
    public class CustomAuthorizeAttribute : TypeFilterAttribute
    {
      /// <summary>
      /// Initializes a new instance of the <see cref="CustomAuthorizeAttribute"/> class.
      /// </summary>
      /// <remarks>
      /// The constructor sets the type of the filter to <see cref="CustomAuthorizeFilter"/>.
      /// </remarks>
        public CustomAuthorizeAttribute() : base(typeof(CustomAuthorizeFilter))
        {
        }
    }
}
