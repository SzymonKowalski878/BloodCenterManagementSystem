﻿using BloodCenterManagementSystem.Logics.Interfaces;
using BloodCenterManagementSystem.Logics.Repositories;
using BloodCenterManagementSystem.Logics.Users.DataHolders;
using BloodCenterManagementSystem.Models;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Text;

namespace BloodCenterManagementSystem.Logics.Users
{
    public class UserLogic:IUserLogic
    {

        private readonly Lazy<IUserRepository> _userRepository;
        protected IUserRepository UserRepositroy => _userRepository.Value;

        private readonly Lazy<IValidator<UserModel>> _userValidator;
        protected IValidator<UserModel> UserValidator => _userValidator.Value;

        private readonly Lazy<IAuthService> _authService;
        protected IAuthService AuthService => _authService.Value;

        public UserLogic(Lazy<IValidator<UserModel>> userValidator,
            Lazy<IUserRepository>userRepository,
            Lazy<IAuthService> authService)
        {
            _userValidator = userValidator;
            _authService = authService;
            _userRepository = userRepository;
        }

        public Result<UserModel> RegisterAccount(RegisterUserrData data)
        {
            if (data == null)
            {
                return Result.Error<UserModel>("Data was null");
            }

            if (string.IsNullOrEmpty(data.Email))
            {
                return Result.Error<UserModel>("Email was null or empty");
            }

            var user = UserRepositroy.GetByEmail(data.Email);

            if(user == null)
            {
                return Result.Error<UserModel>("Unable to find email");
            }

            var passwordValidation = UserValidator.Validate(new UserModel
            {
                Password = data.Password
            },
             options =>
            {
                options.IncludeRuleSets("ValidatePassword");
            }); ;

            if (!passwordValidation.IsValid)
            {
                return Result.Error<UserModel>("Error during password validationn");
            }

            var hashedPassword = AuthService.HashPassword(data.Password);

            if (string.IsNullOrEmpty(hashedPassword))
            {
                return Result.Error<UserModel>("Error during email hashing");
            }

            user.Password = hashedPassword;
            user.Role = "donator";
            UserRepositroy.SaveChanges();

            return Result.Ok(user);
        }

    }
}