using BloodCenterManagementSystem.Logics.Interfaces;
using BloodCenterManagementSystem.Logics.Repositories;
using BloodCenterManagementSystem.Logics.Users.DataHolders;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Security.Principal;
using System.Text;

namespace BloodCenterManagementSystem.Logics.Services
{
    public class EmailConfirmationService: IEmailConfirmationService
    {
        private readonly Lazy<IConfiguration> _configuration;
        protected IConfiguration Configuration => _configuration.Value;

        private readonly Lazy<IUserRepository> _userRepository;
        protected IUserRepository UserRepository => _userRepository.Value;

        public EmailConfirmationService(Lazy<IConfiguration> configuration,
            Lazy<IUserRepository> userRopostiory)
        {
            _configuration = configuration;
            _userRepository = userRopostiory;
        }

        private TokenValidationParameters GetConfimationTokenValidationParameters()
        {
            var secretKey = Configuration["SecretKey"];
            var key = Encoding.ASCII.GetBytes(secretKey);

            return new TokenValidationParameters()
            {
                ValidateLifetime = true,
                ValidateAudience = false,
                ValidateIssuer = false,
                IssuerSigningKey = new SymmetricSecurityKey(key)
            };
        }

        public Result<UserToken> GenerateUserConfirmationToken(string email)
        {
            var secretKey = Configuration["SecretKey"];

            var tokenHanlder = new JwtSecurityTokenHandler();

            var key = Encoding.ASCII.GetBytes(secretKey);

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new Claim[]{
                    new Claim(ClaimTypes.Email,email),
                }),
                Expires = DateTime.Now.AddHours(1),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };

            var token = tokenHanlder.CreateToken(tokenDescriptor);

            var returnValue =  new UserToken()
            {
                Token = tokenHanlder.WriteToken(token)
            };

            return Result.Ok(returnValue);
        }

        public Result<bool> ValidateConfirmationToken(string authToken)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var validationParameters = GetConfimationTokenValidationParameters();

            SecurityToken validatedToken;
            try
            {
                IPrincipal principal = tokenHandler.ValidateToken(authToken, validationParameters, out validatedToken);
            }
            catch(Exception ex)
            {
                return Result.Error<bool>(ex.Message);
            }

            return Result.Ok(true);
        }
    }
}
