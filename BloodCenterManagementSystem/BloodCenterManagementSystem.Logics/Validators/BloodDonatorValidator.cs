using BloodCenterManagementSystem.Logics.Interfaces;
using BloodCenterManagementSystem.Models;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BloodCenterManagementSystem.Logics.Validators
{
    public class BloodDonatorValidator:AbstractValidator<BloodDonatorModel>, Interfaces.IMyValidator
    {
        public bool PeselValidation(string pesel)
        {
            if (pesel.Length != 11)
            {
                return false;
            }

            int[] weights = { 1, 3, 7, 9, 1, 3, 7, 9, 1, 3 };
            var controlNum = 10 - pesel.Zip(weights, (l1, l2) => Int32.Parse(l1.ToString()) * l2).Sum() % 10;

            if (controlNum != Int32.Parse(pesel[10].ToString()))
            {
                return false;
            }

            return true;
        }

        public bool PhoneNumbervalidator(string phoneNumber)
        {
            return Int32.TryParse(phoneNumber, out var number);
        }

        public BloodDonatorValidator()
        {
            //RuleFor(m => m.PhoneNumber)
            //    .Cascade(CascadeMode.StopOnFirstFailure)
            //    .NotEmpty()
            //    .Must(PhoneNumbervalidator)
            //    .Must(x => x.ToString().Length == 9)
            //    .WithMessage("Failure to validate phone number");

            //RuleFor(m => m.HomeAdress)
            //    .Cascade(CascadeMode.StopOnFirstFailure)
            //    .NotEmpty()
            //    .MaximumLength(50)
            //    .WithMessage("Failure to validate home adress");

            //RuleFor(m => m.Pesel)
            //    .Cascade(CascadeMode.StopOnFirstFailure)
            //    .NotEmpty()
            //    .Must(PeselValidation)
            //    .WithMessage("Failure to valdiate pesel");

            RuleSet("ValidatePhoneNumber", () =>
            {
                RuleFor(m => m.PhoneNumber)
                .NotEmpty()
                .Must(PhoneNumbervalidator)
                .Must(x => x.ToString().Length == 9)
                .WithMessage("Failure to validate phone number");
            });

            RuleSet("ValidateHomeAdress", () =>
            {
                RuleFor(m => m.HomeAdress)
                .NotEmpty()
                .MaximumLength(50)
                .WithMessage("Failure to validate home adress");
            });

            RuleSet("ValidatePesel", () =>
            {
                RuleFor(m => m.Pesel)
                .NotEmpty()
                .Must(PeselValidation)
                .WithMessage("Failure to valdiate pesel");
            });


        }


    }
}
