using FluentValidation;
using Hellang.Middleware.ProblemDetails;
using IGaming.Core.Common;
using Microsoft.AspNetCore.Mvc;
using Mysqlx;
using Newtonsoft.Json;
using System;
using System.Net;

namespace IGaming.API.Middlewares
{
    /// <summary>
    /// Middleware for handling exceptions and returning appropriate responses.
    /// </summary>
    public class ExceptionHandlingMiddleware : IMiddleware
    {
        /// <summary>
        /// Invokes the middleware asynchronously.
        /// </summary>
        /// <param name="context">The HTTP context.</param>
        /// <param name="next">The delegate representing the next middleware in the pipeline.</param>
        /// <returns>A task representing the asynchronous operation.</returns>
        public async Task InvokeAsync(HttpContext context, RequestDelegate next)
        {
            try
            {
                await next(context);
            }
            catch (Exception Ex)
            {
                
                context.Response.ContentType = "application/json";
                context.Response.StatusCode = StatusCodes.Status500InternalServerError;
                var resonse = Result.Failure("Bad Request", "Something unexpected happened.", 500);
                await context.Response.WriteAsJsonAsync(resonse);
            }
        }
    }
}
