using BloodCenterManagementSystem.Logics.Interfaces;
using BloodCenterManagementSystem.Logics.Repositories;
using BloodCenterManagementSystem.Logics.Users.DataHolders;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;

namespace BloodCenterManagementSystem.Logics.Services
{
    public class AuthService : IAuthService
    {
        private readonly Lazy<IUserRepository> _userRepository;
        protected IUserRepository UserRepository => _userRepository.Value;

        private readonly Lazy<IConfiguration> _configuration;
        protected IConfiguration Configuration => _configuration.Value;

        public AuthService(Lazy<IUserRepository> userRepository,
            Lazy<IConfiguration> configuration)
        {
            _userRepository = userRepository;
            _configuration = configuration;
        }

        public string HashPassword(string password)
        {
            byte[] salt;
            new RNGCryptoServiceProvider().GetBytes(salt = new byte[16]);

            var pbkdf2 = new Rfc2898DeriveBytes(password,
                salt,
                10000);

            byte[] hash = pbkdf2.GetBytes(20);

            byte[] hashBytes = new byte[36];

            Array.Copy(salt,
                0,
                hashBytes,
                0,
                16);

            Array.Copy(hash,
                0,
                hashBytes,
                16,
                20);

            string hashedPassword = Convert.ToBase64String(hashBytes);

            return hashedPassword;
            
        }

        public bool VerifyPassword(string email,string password)
        {

            var passwordDb = UserRepository.GetUserPassword(email);

            if (string.IsNullOrEmpty(passwordDb) == true)
            {
                return false;
            }

            byte[] hashbytes = Convert.FromBase64String(passwordDb);

            byte[] salt = new byte[16];
            Array.Copy(hashbytes,
                0,
                salt,
                0,
                16);

            var pbkdf2 = new Rfc2898DeriveBytes(password,
                salt,
                10000);

            byte[] hash = pbkdf2.GetBytes(20);

            for (int i = 0; i < 20; i++)
            {
                if (hashbytes[i + 16] != hash[i])
                {
                    return false;
                }
            }

            return true;
        }

        public UserToken GenerateToken(string email, string role)
        {
            var secretKey = Configuration["SecretKey"];

            var tokenHandler = new JwtSecurityTokenHandler();

            var key = Encoding.ASCII.GetBytes(secretKey);

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new Claim[]
                {
                    new Claim(ClaimTypes.Email, email),
                    new Claim("Role",role)
                }),
                Expires = DateTime.Now.AddHours(1),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key),
                SecurityAlgorithms.HmacSha256Signature)
            };

            var token = tokenHandler.CreateToken(tokenDescriptor);

            return new UserToken()
            {
                Token = tokenHandler.WriteToken(token)
            };
        }
    }
}
