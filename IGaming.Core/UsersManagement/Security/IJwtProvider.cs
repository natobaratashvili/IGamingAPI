using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IGaming.Core.UsersManagement.Security
{
    /// <summary>
    /// Represents a provider for JSON Web Tokens (JWT).
    /// </summary>
    public interface IJwtProvider
    {
        /// <summary>
        /// Generates a JWT based on the provided claims.
        /// </summary>
        /// <param name="claims">A dictionary containing the claims to be included in the JWT.</param>
        /// <returns>The generated JWT.</returns>
        string GenerateToken(Dictionary<string, string> claims);
        /// <summary>
        /// Validates the integrity and authenticity of a JWT.
        /// </summary>
        /// <param name="token">The JWT to be validated.</param>
        /// <returns>
        ///   <c>true</c> if the JWT is valid; otherwise, <c>false</c>.
        /// </returns>
        bool ValidateToken(string token);
    }
}
