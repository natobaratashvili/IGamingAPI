using FluentValidation;
using IGaming.Core.UsersManagement.RequestModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IGaming.Core.UsersManagement.Validators
{
    public class UserRegistrationRequestValidator : AbstractValidator<UserRegistrationRequest>
    {
        public UserRegistrationRequestValidator()
        {
            RuleFor(request => request.UserName)
          .NotEmpty().WithMessage("Username cannot be empty")
          .MinimumLength(3).WithMessage("Username must be at least 3 characters long");

            RuleFor(request => request.Email)
                .NotEmpty().WithMessage("Email cannot be empty")
                .EmailAddress().WithMessage("Invalid email address");

            //RuleFor(request => request.Password)
            //    .NotEmpty().WithMessage("Password cannot be empty")
            //    .MinimumLength(6).WithMessage("Password must be at least 6 characters long");

            //RuleFor(request => request.ConfirmPassword)
            //    .Equal(request => request.Password).WithMessage("Passwords do not match");
        }
    }
}
