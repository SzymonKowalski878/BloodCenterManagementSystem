using BloodCenterManagementSystem.Logics.Interfaces;
using BloodCenterManagementSystem.Logics.Repositories;
using BloodCenterManagementSystem.Models;
using EntityFramework.Exceptions.Common;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Text;

namespace BloodCenterManagementSystem.Logics.BloodDonators
{
    public class BloodDonatorLogic:IBloodDonatorLogic
    {
        private readonly Lazy<IBloodDonatorRepository> _bloodDonatorRepository;
        protected IBloodDonatorRepository BloodDonatorRepository => _bloodDonatorRepository.Value;

        private readonly Lazy<IValidator<BloodDonatorModel>> _bloodDonatorValidator;
        protected IValidator<BloodDonatorModel> BloodDonatorValidator => _bloodDonatorValidator.Value;

        private readonly Lazy<IValidator<UserModel>> _userValidator;
        protected IValidator<UserModel> UserValidator => _userValidator.Value;

        private readonly Lazy<IBloodTypeRepository> _bloodTypeRepository;
        protected IBloodTypeRepository BloodTypeRepository => _bloodTypeRepository.Value;

        public BloodDonatorLogic(Lazy<IBloodDonatorRepository> bloodDonatorRepository,
            Lazy<IValidator<BloodDonatorModel>> bloodDonatorValidator,
            Lazy<IBloodTypeRepository> bloodTypeRepository,
            Lazy<IValidator<UserModel>> userValidator)
        {
            _bloodDonatorRepository = bloodDonatorRepository;
            _bloodDonatorValidator = bloodDonatorValidator;
            _bloodTypeRepository = bloodTypeRepository;
            _userValidator = userValidator;
        }

        public Result<BloodDonatorModel> RegisterBloodDonator (BloodDonatorModel data)
        {
            if (data == null || data.User == null)
            {
                return Result.Error<BloodDonatorModel>("Data was null");
            }

            var donatorValidation = BloodDonatorValidator.Validate(data);

            if (!donatorValidation.IsValid)
            {
                return Result.Error<BloodDonatorModel>(donatorValidation.Errors);
            }

            var nameValidation = UserValidator.Validate(data.User, options =>
            {
                options.IncludeRuleSets("ValidateNames");
            });

            if (!nameValidation.IsValid)
            {
                return Result.Error<BloodDonatorModel>("Error during user name validation");
            }

            if (!string.IsNullOrEmpty(data.User.Email))
            {
                var emilValidation = UserValidator.Validate(data.User, options =>
                {
                    options.IncludeRuleSets("ValidateEmail");
                });

                if (!emilValidation.IsValid)
                {
                    return Result.Error<BloodDonatorModel>(emilValidation.Errors);
                }
            }
            //if email is empty make sure it is stored as null in database
            else
            {
                data.User.Email = null;
            }

            var bloodType = BloodTypeRepository.GetByBloodTypeName(data.BloodType.BloodTypeName);

            if (bloodType == null)
            {
                return Result.Error<BloodDonatorModel>("Unable to find blood type with such name");
            }

            data.BloodType = null;
            data.BloodTypeId = bloodType.Id;

            try
            {
                BloodDonatorRepository.Add(data);
                BloodDonatorRepository.SaveChanges();
            }
            catch(UniqueConstraintException ex)
            {
                return Result.Error<BloodDonatorModel>(ex.Message);
            }
            return Result.Ok(data);
        }
    }
}
