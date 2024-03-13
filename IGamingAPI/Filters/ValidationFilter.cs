using FluentValidation;
using IGaming.Core.Common;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace IGaming.API.Filters
{
    /// <summary>
    /// Filter for validating the input data of a specified type using a corresponding validator.
    /// </summary>
    /// <typeparam name="T">The type of the data to be validated.</typeparam>
    public class ValidationFilter<T> : IAsyncActionFilter
    {
        private readonly IServiceProvider _serviceProvider;
        /// <summary>
        /// Initializes a new instance of the <see cref="ValidationFilter{T}"/> class.
        /// </summary>
        /// <param name="serviceProvider">The service provider.</param>
        public ValidationFilter(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
        }

        /// <summary>
        /// Validates the input data asynchronously.
        /// </summary>
        /// <param name="context">The action executing context.</param>
        /// <param name="next">The action execution delegate.</param>
        /// <returns>A task representing the asynchronous operation.</returns>
        /// <remarks>
        /// This filter iterates over the action arguments and checks if they match the specified type <typeparamref name="T"/>.
        /// If a matching argument is found, it retrieves the corresponding validator from the service provider.
        /// The validator is then used to validate the argument's data.
        /// If validation fails, a response with status code 400 (Bad Request) and validation errors is returned.
        /// </remarks>
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