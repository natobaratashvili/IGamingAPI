using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IGaming.Core.UsersManagement.Security
{
    public interface IJwtProvider
    {
        string GenerateToken(Dictionary<string, string> claims);
        bool ValidateToken(string token);
    }
}
