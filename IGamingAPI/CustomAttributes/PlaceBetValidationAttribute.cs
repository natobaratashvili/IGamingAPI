using IGaming.API.Filters;
using IGaming.Core.Bets.RequestModels;
using Microsoft.AspNetCore.Mvc;

namespace IGaming.API.CustomAttributes
{
    public class PlaceBetValidationAttribute : TypeFilterAttribute
    {
        public PlaceBetValidationAttribute() : base(typeof(ValidationFilter<PlaceBetRequest>))
        {
        }
    }
}
