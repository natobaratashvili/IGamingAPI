using FluentValidation;
using IGaming.Core.UsersManagement.RequestModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IGaming.Core.UsersManagement.Validators
{
    public class AuthenticateRequestValidator : AbstractValidator<UserAuthenticateRequest>
    {
        public AuthenticateRequestValidator()
        {

        }
    }
}
