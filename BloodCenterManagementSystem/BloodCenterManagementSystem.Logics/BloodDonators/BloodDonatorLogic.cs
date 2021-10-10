using BloodCenterManagementSystem.Logics.BloodDonators.DataHolders;
using BloodCenterManagementSystem.Logics.Donations.DataHolders;
using BloodCenterManagementSystem.Logics.Interfaces;
using BloodCenterManagementSystem.Logics.Repositories;
using BloodCenterManagementSystem.Logics.Users.DataHolders;
using BloodCenterManagementSystem.Models;
using EntityFramework.Exceptions.Common;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
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

        private readonly Lazy<IAuthService> _authService;
        protected IAuthService AuthService => _authService.Value;

        public BloodDonatorLogic(Lazy<IBloodDonatorRepository> bloodDonatorRepository,
            Lazy<IValidator<BloodDonatorModel>> bloodDonatorValidator,
            Lazy<IBloodTypeRepository> bloodTypeRepository,
            Lazy<IValidator<UserModel>> userValidator,
            Lazy<IAuthService> authService)
        {
            _bloodDonatorRepository = bloodDonatorRepository;
            _bloodDonatorValidator = bloodDonatorValidator;
            _bloodTypeRepository = bloodTypeRepository;
            _userValidator = userValidator;
            _authService = authService;
        }

        public Result<BloodDonatorModel> RegisterBloodDonator (BloodDonatorModel data)
        {
            if (data == null || data.User == null)
            {
                return Result.Error<BloodDonatorModel>("Data was null");
            }

            var donatorValidation = BloodDonatorValidator.Validate(data,options=> {
                options.IncludeAllRuleSets();
            });

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

        public Result<BloodDonatorModel> ReturnDonatorInformation(int id)
        {
            var bloodDonator = BloodDonatorRepository.ReturnDonatorInfo(id);

            if(bloodDonator== null || bloodDonator.User == null)
            {
                return Result.Error<BloodDonatorModel>("Unable to find user with that id");
            }

            var bloodType = BloodTypeRepository.GetById(bloodDonator.BloodTypeId);

            if (bloodType == null)
            {
                return Result.Error<BloodDonatorModel>("Unable to get blood type by bloodTypeId");
            }

            return Result.Ok(bloodDonator);
        }

        public Result<BloodDonatorModel> UpdateUserData(UpdateUserData data)
        {
            if (string.IsNullOrEmpty(data.Email) && string.IsNullOrEmpty(data.Password) && string.IsNullOrEmpty(data.HomeAdress) && string.IsNullOrEmpty(data.PhoneNumber))
            {
                return Result.Error<BloodDonatorModel>("All update properites were empty");
            }

            var user = new UserModel
            {
                Email = data?.Email,
                Password = data?.Password
            };

            var toUpdate = BloodDonatorRepository.ReturnDonatorInfo(data.Id);

            if (toUpdate == null)
            {
                return Result.Error<BloodDonatorModel>("Unable to find user with id passed in the data");
            }

            if (!string.IsNullOrEmpty(data.Email))
            {
                var emailValidation = UserValidator.Validate(user, options =>
                {
                    options.IncludeRuleSets("ValidateEmail");
                });

                if (!emailValidation.IsValid)
                {
                    return Result.Error<BloodDonatorModel>(emailValidation.Errors);
                }

                toUpdate.User.Email = user.Email;
                BloodDonatorRepository.SaveChanges();
            }

            if (!string.IsNullOrEmpty(data.Password))
            {
                var passwordValidation = UserValidator.Validate(user, options =>
                {
                    options.IncludeRuleSets("ValidatePassword");
                });

                if (!passwordValidation.IsValid)
                {
                    return Result.Error<BloodDonatorModel>(passwordValidation.Errors);
                }

                var hashedPassword = AuthService.HashPassword(user.Password);

                if (String.IsNullOrEmpty(hashedPassword))
                {
                    return Result.Error<BloodDonatorModel>("Error during password hashing");
                }

                toUpdate.User.Password = hashedPassword;
                BloodDonatorRepository.SaveChanges();
            }

            var donator = new BloodDonatorModel
            {
                HomeAdress = data?.HomeAdress,
                PhoneNumber = data?.PhoneNumber
            };

            if (!string.IsNullOrEmpty(data.HomeAdress))
            {
                var homeAdressValidation = BloodDonatorValidator.Validate(donator, options =>
                {
                    options.IncludeRuleSets("ValidateHomeAdress");
                });

                if (!homeAdressValidation.IsValid)
                {
                    return Result.Error<BloodDonatorModel>(homeAdressValidation.Errors);
                }

                toUpdate.HomeAdress = donator.HomeAdress;
                BloodDonatorRepository.SaveChanges();
            }

            if (!string.IsNullOrEmpty(data.PhoneNumber))
            {
                var phoneNumberValidation = BloodDonatorValidator.Validate(donator, options =>
                {
                    options.IncludeRuleSets("ValidatePhoneNumber");
                });

                if (!phoneNumberValidation.IsValid)
                {
                    return Result.Error<BloodDonatorModel>(phoneNumberValidation.Errors);
                }

                toUpdate.PhoneNumber = donator.PhoneNumber;
                BloodDonatorRepository.SaveChanges();
            }

            return Result.Ok(toUpdate);
        }

        public Result<BloodDonatorModel> ReturnDonatorByPesel(PeselHolder pesel)
        {
            if (string.IsNullOrEmpty(pesel.Pesel))
            {
                return Result.Error<BloodDonatorModel>("Data was null");
            }

            var donator = BloodDonatorRepository.ReturnDoantorInfoByPesel(pesel.Pesel);

            if(donator == null)
            {
                return Result.Error<BloodDonatorModel>("Unable to find donator with such pesel");
            }

            return Result.Ok(donator);
        }

        public Result<IEnumerable<BloodDonatorModel>> GetAll()
        {
            var result = BloodDonatorRepository.GetAll();

            if (result.Count() == 0)
            {
                return Result.Error<IEnumerable<BloodDonatorModel>>("No users found");
            }

            return Result.Ok(result);
        }
    }
}
