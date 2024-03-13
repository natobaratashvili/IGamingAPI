using IGaming.API.Filters;
using Microsoft.AspNetCore.Mvc;

namespace IGaming.API.CustomAttributes
{
    public class CustomAuthorizeAttribute : TypeFilterAttribute
    {
        public CustomAuthorizeAttribute() : base(typeof(CustomAuthorizeFilter))
        {
        }
    }
}
