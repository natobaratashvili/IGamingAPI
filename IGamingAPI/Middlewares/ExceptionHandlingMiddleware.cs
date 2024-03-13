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
    public class ExceptionHandlingMiddleware : IMiddleware
    { 
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
