using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IGaming.Core.UsersManagement.Security
{
    public interface IHasher
    {
        public string Compute(string password, string username);
        public bool Verify(string password, string username,  string hashedInput);
    }
}
