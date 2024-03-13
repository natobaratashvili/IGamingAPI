using System.Security.Cryptography;
using System.Text;
using IGaming.Core.UsersManagement.Security;

namespace IGaming.Infrastructure.Security.Hashing
{
    public class Hasher : IHasher
    {
        public string Compute(string password, string username )
        {
            using var sha256 = SHA256.Create();
            var concat = password + username;
            byte[] hashedBytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(concat));
            return Convert.ToBase64String(hashedBytes);
        }

        public bool Verify(string password , string username,  string hashedInput)
        {
            string hashedInputToVerify = Compute(password, username);
            return string.Equals(hashedInputToVerify, hashedInput, StringComparison.Ordinal);
        }
    }
}
