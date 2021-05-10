using BloodCenterManagementSystem.Logics.Interfaces;
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

        public Result<UserModel> RegisterAccount(UserEmailAndPassword data)
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
            });

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
            user.Role = "Donator";
            UserRepositroy.SaveChanges();

            return Result.Ok(user);
        }

        public Result<UserToken> Login(UserIdAndPassword data)
        {
            if(data == null)
            {
                return Result.Error<UserToken>("Data was null");
            }

            var authResult = AuthService.VerifyPassword(data.Id, data.Password);

            if (!authResult)
            {
                return Result.Error<UserToken>("Error during authentication");
            }

            var user = UserRepositroy.GetById(data.Id);

            if (user == null)
            {
                return Result.Error<UserToken>("Error while trying to get user from database");
            }


            var tokenData = AuthService.GenerateToken(user.Id, user.Role);

            if (tokenData == null)
            {
                return Result.Error<UserToken>("Error during token generation");
            }

            return Result.Ok(tokenData);
        }
    }
}
