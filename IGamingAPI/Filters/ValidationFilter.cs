using FluentValidation;
using IGaming.Core.Common;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace IGaming.API.Filters
{
    public class ValidationFilter<T> : IAsyncActionFilter
    {
        private readonly IServiceProvider _serviceProvider;

        public ValidationFilter(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
        }

   
        public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {
            var arguments = context.ActionArguments.Values;
            foreach (var argument in arguments)
            {
                if (argument!.GetType() != typeof(T)) continue;
                Type genericType = typeof(IValidator<>).MakeGenericType(argument!.GetType());
                var validator = _serviceProvider.GetService(genericType)!;
                if (validator != null)
                {
                    var validationResult = ((IValidator<T>)validator).Validate((T)argument)!;
                    if (!validationResult.IsValid)
                    {
                        context.HttpContext.Response.ContentType = "application/json";
                        context.HttpContext.Response.StatusCode = StatusCodes.Status400BadRequest;
                        var response = Result.Failure(400);
                        
                        foreach(var err in validationResult.Errors)
                        {
                            response.Errors[err.PropertyName] = err.ErrorMessage;
                        }
                        await context.HttpContext.Response.WriteAsJsonAsync(response);
                        return;
                    }
                }
            }
            await next();
        }    
    }
}