using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IGaming.Core.UsersManagement.Security
{
    public interface IHasher
    {
        public string Compute(string input);
        public bool Verify(string input, string hashedInput);
    }
}
