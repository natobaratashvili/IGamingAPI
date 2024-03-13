using IGaming.Infrastructure.Security.Jwt;
using Microsoft.AspNetCore.Mvc;
using System.Runtime.CompilerServices;

namespace IGaming.API.Extensions
{
    /// <summary>
    /// Extension methods for HttpRequest objects.
    /// </summary>
    public static class RequestExtensions
    {
        /// <summary>
        /// Gets the current username from the request's authorization header.
        /// </summary>
        /// <param name="httpRequest">The HttpRequest object.</param>
        /// <returns>The username extracted from the authorization header, or an empty string if not found.</returns>
        public static string GetCurrentUsername(this HttpRequest httpRequest)
        {
            var tokenWithBearer = httpRequest.Headers["Authorization"].ToString();
            if (string.IsNullOrEmpty(tokenWithBearer)) return string.Empty;
            
            var token = tokenWithBearer.Split(" ")[1];
            var claims = JwtProvider.GetClaims(token);
            var username = claims.FirstOrDefault( c => c.Key == "username");
            if(username.Key!= null)
            {
                return username.Value;
            }
            return string.Empty;
        }
    }
}
