using System.Security.Cryptography;
using System.Text;
using IGaming.Core.UsersManagement.Security;

namespace IGaming.Infrastructure.Security.Hashing
{
    public class Hasher : IHasher
    {
        public string Compute(string input)
        {
            using var sha256 = SHA256.Create();
            byte[] hashedBytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(input));
            return Convert.ToBase64String(hashedBytes);
        }

        public bool Verify(string input, string hashedInput)
        {
            string hashedInputToVerify = Compute(input);
            return string.Equals(hashedInputToVerify, hashedInput, StringComparison.Ordinal);
        }
    }
}
