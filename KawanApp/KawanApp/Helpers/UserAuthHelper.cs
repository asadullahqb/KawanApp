using FluentValidation;
using KawanApp.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace KawanApp.Helpers
{
    class UserAuthValidator : AbstractValidator<UserAuthentication>
    {
        public UserAuthValidator()
        {
            RuleFor(user => user.Email)
                .EmailAddress()
                .WithMessage("Email is not in correct format");

            RuleFor(user => user.Password)
                .NotNull()
                .NotEmpty()
                .WithMessage("Password can not be empty")
                .Length(6, 20)
                .WithMessage("Password must be at least 6 characters long");
        }
    }
}
