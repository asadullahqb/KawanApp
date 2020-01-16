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
            RuleFor(user => user.StudentId)
                .NotNull()
                .NotEmpty()
                .WithMessage("Student Id cannot be empty");

            RuleFor(user => user.Password)
                .NotNull()
                .NotEmpty()
                .WithMessage("Password cannot be empty")
                .Length(6, 20)
                .WithMessage("Password must be at least 6 characters long");
        }
    }
}
