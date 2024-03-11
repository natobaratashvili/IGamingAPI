using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IGaming.Core.UsersManagement.Dtos
{
    public class UserProfileDto
    {
        public int Id { get; set; }
        public string Email { get; set; } = string.Empty;
        public string Username { get; set; } = string.Empty;
        public string Guid { get; set; } = string.Empty;
        public string HashedPassword { get; set; } = string.Empty;
        public DateTime CreateDateAtUtc { get; set; }
        public double Balance { get; set; }
        public double TotalBet { get; set; }
    }

    
}
