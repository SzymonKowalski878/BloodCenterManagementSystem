using BloodCenterManagementSystem.Logics.Interfaces;
using BloodCenterManagementSystem.Models;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;

namespace BloodCenterManagementSystem.Logics.Validators
{
    public class UserValidator:AbstractValidator<UserModel>, Interfaces.IMyValidator
    {
        public bool ValidatePasswordStrength(string password)
        {
            var pattern = @"^.*((?=.*[!@#$%^&*()\-_=+{};:,<.>]){1})(?=.*\d)((?=.*[a-z]){1})((?=.*[A-Z]){1}).*$";

            return Regex.IsMatch(password,
                pattern,
                RegexOptions.ECMAScript);
        }

        public UserValidator()
        {
            RuleSet("ValidatePassword", () =>
            {
                RuleFor(m => m.Password)
                .NotEmpty()
                .Must(ValidatePasswordStrength)
                .WithMessage("Error during password validation");
            });

            RuleSet("ValidateEmail", ()=>{
                RuleFor(m => m.Email)
                .EmailAddress()
                .NotEmpty()
                .NotNull()
                .WithMessage("Error during email validation");
            });

            RuleSet("ValidateNames", () =>
            {
                RuleFor(m => m.FirstName)
                .NotEmpty()
                .MaximumLength(30)
                .WithMessage("Failure to validate firstname");

                RuleFor(m => m.Surname)
                .NotEmpty()
                .MaximumLength(30)
                .WithMessage("Failure to validate surname");
            });
        }
    }
}
