using BloodCenterManagementSystem.Logics.Donations.DataHolders;
using BloodCenterManagementSystem.Logics.Interfaces;
using BloodCenterManagementSystem.Logics.Repositories;
using BloodCenterManagementSystem.Logics.Users.DataHolders;
using BloodCenterManagementSystem.Models;
using EntityFramework.Exceptions.Common;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Text;

namespace BloodCenterManagementSystem.Logics.Users
{
    public class UserLogic:IUserLogic
    {

        private readonly Lazy<IUserRepository> _userRepository;
        protected IUserRepository UserRepository => _userRepository.Value;

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

        public Result<UserToken> Login(UserEmailAndPassword data)
        {
            if(data == null)
            {
                return Result.Error<UserToken>("Data was null");
            }

            var authResult = AuthService.VerifyPassword(data.Email, data.Password);

            if (!authResult)
            {
                return Result.Error<UserToken>("Error during authentication");
            }

            var user = UserRepository.GetByEmail(data.Email);

            if (user == null)
            {
                return Result.Error<UserToken>("Error while trying to get user from database");
            }

            if (!user.EmailConfirmed)
            {
                return Result.Error<UserToken>("User must confirm email");
            }

            var tokenData = AuthService.GenerateToken(user.Email, user.Role,user.Id);

            if (tokenData == null)
            {
                return Result.Error<UserToken>("Error during token generation");
            }

            return Result.Ok(tokenData);
        }

        public Result<UserToken> RenewToken(string email)
        {
            var user = UserRepository.GetByEmail(email);
            if (user == null)
            {
                return Result.Error<UserToken>("Unable to find user with such id");
            }

            var token = AuthService.GenerateToken(user.Email, user.Role,user.Id);

            if (token == null)
            {
                return Result.Error<UserToken>("Error during token generation");
            }

            return Result.Ok(token);
        }

        public Result<UserModel> RegisterWokrer(UserModel data, string role)
        {
            if (data == null)
            {
                return Result.Error<UserModel>("Data was null");
            }

            var userValidation = UserValidator.Validate(data, options =>
            {
                options.IncludeAllRuleSets();
            });

            if (!userValidation.IsValid)
            {
                return Result.Error<UserModel>("Error during user data validation");
            }

            var hashedPassword = AuthService.HashPassword(data.Password);

            if (string.IsNullOrEmpty(hashedPassword))
            {
                return Result.Error<UserModel>("Error during password hashing");
            }

            data.Password = hashedPassword;
            data.EmailConfirmed = true;
            data.Role = role;

            try
            {
                UserRepository.Add(data);
                UserRepository.SaveChanges();
            }
            catch(UniqueConstraintException ex)
            {
                return Result.Error<UserModel>(ex.Message);
            }

            return Result.Ok(data);
        }

        private string ReadClaim(string token, string claimType)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var securityToken = tokenHandler.ReadToken(token) as JwtSecurityToken;

            var stringClaimValue = securityToken.Claims.First(claim => claim.Type == claimType).Value;
            return stringClaimValue;
        }

        public Result<string> RegisterAccount(string email,string authToken,string password)
        {
            var tokenEmail = ReadClaim(authToken, "email");
            if (string.IsNullOrEmpty(tokenEmail) || email != tokenEmail)
            {
                return Result.Error<string>("Error during validation");
            }

            var user = UserRepository.GetByEmail(email);

            if (user == null)
            {
                return Result.Error<string>("Error during validation");
            }

            var passwordValidation = UserValidator.Validate(new UserModel
            {
                Password = password
            },
            options =>
            {
                options.IncludeRuleSets("ValidatePassword");
            });

            if (!passwordValidation.IsValid)
            {
                return Result.Error<string>("Error during password validationn");
            }

            var hashedPassword = AuthService.HashPassword(password);

            if (string.IsNullOrEmpty(hashedPassword))
            {
                return Result.Error<string>("Error during email hashing");
            }

            user.Password = hashedPassword;
            user.EmailConfirmed = true;
            user.Role = "Donator";

            try 
            { 
                UserRepository.SaveChanges();
            }
            catch(UniqueConstraintException ex)
            {
                return Result.Error<string>(ex.Message);
            }

            return Result.Ok(email);
        }

        public Result<UserModel> DeleteAccount(int id)
        {
            var user = UserRepository.GetById(id);

            if (user == null)
            {
                return Result.Error<UserModel>("Unable to find worker with such id");
            }

            if(!(user.Role=="Worker" || user.Role == "Admin"))
            {
                return Result.Error<UserModel>("Not allowed to delete account other than worker or admin account");
            }

            try
            {
                UserRepository.Delete(user);
                UserRepository.SaveChanges();
            }
            catch(Exception ex)
            {
                return Result.Error<UserModel>(ex.Message);
            }

            return Result.Ok(user);
        }

        public Result<IEnumerable<UserModel>> ReturnAllWorkers()
        {
            var result = UserRepository.ReturnAllWorkers();

            if (result.Count() < 1)
            {
                return Result.Error<IEnumerable<UserModel>>("Unable to find any workers");
            }

            return Result.Ok(result);
        }

        public Result<UserModel> ReturnWorkerById(int id)
        {
            var user = UserRepository.GetById(id);

            if (user == null)
            {
                return Result.Error<UserModel>("Unable to find user with such id");
            }

            if(!(user.Role == "Worker" || user.Role == "Admin"))
            {
                return Result.Error<UserModel>("This endpoint returns only workers and admins");
            }

            return Result.Ok(user);
        }
    }
}
