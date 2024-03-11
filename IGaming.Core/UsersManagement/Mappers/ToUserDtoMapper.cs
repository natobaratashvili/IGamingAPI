using IGaming.Core.UsersManagement.Dtos;
using IGaming.Core.UsersManagement.RequestModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IGaming.Core.UsersManagement.Mappers
{
    public static class ToUserDtoMapper
    {
        public static UserProfileDto ToUserDto(this UserRegistrationRequest registrationRequest)
        {
            return new UserProfileDto
            {
                Email = registrationRequest.Email,
                Username = registrationRequest.UserName,
                Guid = Guid.NewGuid().ToString(),
                  

            };

        }
    }
}
