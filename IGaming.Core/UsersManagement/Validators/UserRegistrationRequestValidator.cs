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
            RuleFor(x => x.UserName)
            .NotEmpty().WithMessage("Username cannot be empty")
            .Matches(@"^[a-zA-Z0-9_]+$").WithMessage("Username must only contain letters, numbers, or underscores");

            RuleFor(x => x.Email)
                .NotEmpty().WithMessage("Email cannot be empty")
                .EmailAddress().WithMessage("Invalid email format");

            RuleFor(x => x.Password)
                .NotEmpty().WithMessage("Password cannot be empty")
                .Matches(@"^(?=.*[A-Z])(?=.*[a-z])(?=.*\d).{8,}$")
                .WithMessage("Password must contain at least 8 characters, including one uppercase letter, one lowercase letter, and one digit");

            RuleFor(x => x.ConfirmPassword)
                .NotEmpty().WithMessage("Confirm Password cannot be empty")
                .Equal(x => x.Password).WithMessage("Passwords do not match");
        }
    }
}
