using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IGaming.Core.UsersManagement.Security
    {
    /// <summary>
    /// Interface for hashing passwords and verifying hashed passwords.
    /// </summary>
    public interface IHasher
    {
        /// <summary>
        /// Computes the hash of the given password.
        /// </summary>
        /// <param name="password">The password to hash.</param>
        /// <param name="username">The associated username as a salt.</param>
        /// <returns>The hashed password.</returns>
        public string Compute(string password, string username);
        /// <summary>
        /// Verifies whether the provided password matches the hashed input.
        /// </summary>
        /// <param name="password">The password to verify.</param>
        /// <param name="username">The associated username as a salt.</param>
        /// <param name="hashedInput">The hashed password to compare against.</param>
        /// <returns>True if the password matches the hashed input; otherwise, false.</returns>
        public bool Verify(string password, string username,  string hashedInput);
    }
}
