using IGaming.Core.UsersManagement.Dtos;
using IGaming.Core.UsersManagement.RequestModels;
using IGaming.Core.UsersManagement.ResponseModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IGaming.Core.UsersManagement.Mappers
{
    public static class ToProfileResponse
    {
        public static UserProfileResponse ToProfile(this UserProfileDto dto)
        {
            return new UserProfileResponse(dto.Id, dto.Guid, dto.Email, dto.Username, dto.Balance, dto.TotalBet);

        }
    }
}
