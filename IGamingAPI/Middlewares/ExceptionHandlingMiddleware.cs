using FluentValidation;
using IGaming.Core.Common;
using Newtonsoft.Json;
using System;
using System.Net;

namespace IGaming.API.Middlewares
{
    public class ExceptionHandlingMiddleware : IMiddleware
    { 
        public async Task InvokeAsync(HttpContext context, RequestDelegate next)
        {
            try
            {
                await next(context);
            }
            catch (ValidationException ex)
            {
                context.Response.ContentType = "application/json";
                context.Response.StatusCode = 400;
                var messages = ex.Errors.Select(x => x.ErrorMessage).ToList();
                var validationFailureResponse = Response<object>.Failure(messages);
                await context.Response.WriteAsJsonAsync(validationFailureResponse);
            }
            catch (Exception)
            {
                context.Response.ContentType = "application/json";
                context.Response.StatusCode = StatusCodes.Status500InternalServerError;

               var result = Response<object>.Failure("An unexpected error occurred.");

                await context.Response.WriteAsJsonAsync(result);
            }
        }
    }
}
