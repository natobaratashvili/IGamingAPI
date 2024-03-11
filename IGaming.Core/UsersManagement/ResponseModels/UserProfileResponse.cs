using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IGaming.Core.UsersManagement.ResponseModels
{
    public record UserProfileResponse(int Id, string guid, string Email, string UserName, double Balance, double TotalBets);

}
