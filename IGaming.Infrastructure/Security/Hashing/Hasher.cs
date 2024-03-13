using System.Security.Cryptography;
using System.Text;
using IGaming.Core.UsersManagement.Security;

namespace IGaming.Infrastructure.Security.Hashing
{
    /// <summary>
    /// Provides functionality for computing and verifying password hashes.
    /// </summary>
    public class Hasher : IHasher
    {
        /// <summary>
        /// Computes the hash of the password concatenated with the username.
        /// </summary>
        /// <param name="password">The password to hash.</param>
        /// <param name="username">The username to concatenate with the password.</param>
        /// <returns>The base64-encoded hash of the concatenated password and username.</returns>
        /// <remarks>
        /// This method computes the SHA-256 hash of the provided password concatenated with the username.
        /// It combines the password and username to create a unique input for hashing, enhancing security.
        /// </remarks
        public string Compute(string password, string username )
        {
            using var sha256 = SHA256.Create();
            var concat = password + username;
            byte[] hashedBytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(concat));
            return Convert.ToBase64String(hashedBytes);
        }
        /// <summary>
        /// Verifies whether a password matches a hashed input.
        /// </summary>
        /// <param name="password">The password to verify.</param>
        /// <param name="username">The username associated with the password.</param>
        /// <param name="hashedInput">The hashed input to compare against.</param>
        /// <returns>True if the password matches the hashed input; otherwise, false.</returns>
        /// <remarks>
        /// This method computes the hash of the provided password concatenated with the username and compares it with the provided hashed input.
        /// It returns true if the computed hash matches the hashed input, indicating a successful verification of the password.
        /// </remarks>
        public bool Verify(string password , string username,  string hashedInput)
        {
            string hashedInputToVerify = Compute(password, username);
            return string.Equals(hashedInputToVerify, hashedInput, StringComparison.Ordinal);
        }
    }
}
