using BloodCenterManagementSystem.Logics;
using BloodCenterManagementSystem.Logics.Interfaces;
using BloodCenterManagementSystem.Logics.Repositories;
using BloodCenterManagementSystem.Logics.Services;
using BloodCenterManagementSystem.Logics.Users;
using BloodCenterManagementSystem.Logics.Users.DataHolders;
using BloodCenterManagementSystem.Models;
using EntityFramework.Exceptions.Common;
using FluentValidation;
using FluentValidation.Results;
using Microsoft.Extensions.Configuration;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Xunit;

namespace BloodCenterManagementSystem.Tests.LogicTests
{
    public class UserLogicTests
    {
        private readonly UserLogic Logic;
        private readonly Mock<IUserRepository> Repository = new Mock<IUserRepository>();
        private readonly Mock<IConfiguration> Configuration = new Mock<IConfiguration>();
        private readonly Mock<IValidator<UserModel>> Validator = new Mock<IValidator<UserModel>>();
        private readonly Mock<IAuthService> AuthService = new Mock<IAuthService>();

        public UserLogicTests()
        {
            var lazyRepository = new Lazy<IUserRepository>(() => Repository.Object);
            var lazyConfiguration = new Lazy<IConfiguration>(() => Configuration.Object);
            var lazyValidator = new Lazy<IValidator<UserModel>>(() => Validator.Object);
            var authService = new AuthService(lazyRepository, lazyConfiguration);
            var lazyAuthService = new Lazy<IAuthService>(AuthService.Object);

            Logic = new UserLogic(lazyValidator, lazyRepository, lazyAuthService);
            
        }

        [Fact]
        public void LoginReturnsErrorWhenDataIsNull()
        {
            //Arrange
            UserEmailAndPassword data = null;

            //Act
            var result = Logic.Login(data);

            //Assert
            Assert.False(result.IsSuccessfull);
            Assert.Contains(result.ErrorMessages, item => item.Message == "Data was null");
        }

        [Fact]
        public void LoginReturnsErrorWhenAuthReturnsFalse()
        {
            //Arrange
            AuthService.Setup(x => x.VerifyPassword("test", "Hej"))
                .Returns(false);

            //Act
            var result = Logic.Login(new UserEmailAndPassword
            {
                Email = "test",
                Password = "Hej"
            });

            //Assert
            Assert.False(result.IsSuccessfull);
            Assert.Contains(result.ErrorMessages, item => item.Message == "Error during authentication");
        }

        [Fact]
        public void LoginReturnsErrorWhenUnableToFindUser()
        {
            //Arrange
            AuthService.Setup(x => x.VerifyPassword("test", "Hej"))
                .Returns(true);

            //Act
            var result = Logic.Login(new UserEmailAndPassword
            {
                Email = "test",
                Password = "Hej"
            });

            //Assert
            Assert.False(result.IsSuccessfull);
            Assert.Contains(result.ErrorMessages, item => item.Message == "Error while trying to get user from database");
        }

        [Fact]
        public void LoginReturnsErrorWhenEmailIsntConfirmed()
        {
            //Arrange
            AuthService.Setup(x => x.VerifyPassword("test", "Hej"))
                .Returns(true);

            var user = new UserModel
            {
                Email = "test",
                Password = "Hej",
                EmailConfirmed = false
            };

            Repository.Setup(x => x.GetByEmail("test"))
                .Returns(user);

            //Act
            var result = Logic.Login(new UserEmailAndPassword
            {
                Email = "test",
                Password = "Hej"
            });

            //Assert
            Assert.False(result.IsSuccessfull);
            Assert.Contains(result.ErrorMessages, item => item.Message == "User must confirm email");
        }

        [Fact]
        public void LoginReturnsErrorWhenGenerateTokenReturnsNull()
        {
            //Arrange
            AuthService.Setup(x => x.VerifyPassword("test", "Hej"))
                .Returns(true);

            var user = new UserModel
            {
                Id=0,
                Email = "test",
                Password = "Hej",
                EmailConfirmed = true,
                Role=null
            };

            Repository.Setup(x => x.GetByEmail("test"))
                .Returns(user);

            AuthService.Setup(x => x.GenerateToken(user.Email, user.Role, user.Id))
                .Returns((UserToken)null);

            //Act
            var result = Logic.Login(new UserEmailAndPassword
            {
                Email = "test",
                Password = "Hej"
            });

            //Assert
            Assert.False(result.IsSuccessfull);
            Assert.Contains(result.ErrorMessages, item => item.Message == "Error during token generation");
        }

        [Fact]
        public void LoginReturnsTokenWhenEverythingIsOk()
        {
            //Arrange
            AuthService.Setup(x => x.VerifyPassword("test", "Hej"))
                .Returns(true);

            var user = new UserModel
            {
                Id = 0,
                Email = "test",
                Password = "Hej",
                EmailConfirmed = true,
                Role = null
            };

            Repository.Setup(x => x.GetByEmail("test"))
                .Returns(user);

            AuthService.Setup(x => x.GenerateToken(user.Email, user.Role, user.Id))
                .Returns(new UserToken
                {
                    Token="someTokenXYZ"
                });

            //Act
            var result = Logic.Login(new UserEmailAndPassword
            {
                Email = "test",
                Password = "Hej"
            });

            //Assert
            Assert.True(result.IsSuccessfull);
            Assert.Contains(result.Value.Token, "someTokenXYZ");
            Assert.Equal(0, (int)result.ErrorMessages.Count());
        }

        [Fact]
        public void RenewTokenReturnsErrorWhenUnableToFindUser()
        {
            //Arrange
            Repository.Setup(x => x.GetByEmail("test"))
                .Returns((UserModel)null);

            //Act
            var result = Logic.RenewToken("test");

            //Assert
            Assert.False(result.IsSuccessfull);
            Assert.Contains(result.ErrorMessages, item => item.Message == "Unable to find user with such id");
        }

        [Fact]
        public void RenewTokenReturnsErrorWhenGenerateTokenReturnsNull()
        {
            //Arrange
            var user = new UserModel
            {
                Email = "test",
                Role = null,
                Id = 1
            };

            Repository.Setup(x => x.GetByEmail("test"))
                .Returns(user);

            AuthService.Setup(x => x.GenerateToken(user.Email, user.Role, user.Id))
                .Returns((UserToken)null);

            //Act
            var result = Logic.RenewToken("test");

            //Assert
            Assert.False(result.IsSuccessfull);
            Assert.Contains(result.ErrorMessages, item => item.Message == "Error during token generation");
        }

        [Fact]
        public void RenewTokenReturnsTokenWheneEverythingIsOk()
        {
            //Arrange
            var user = new UserModel
            {
                Email = "test",
                Role = null,
                Id = 1
            };

            Repository.Setup(x => x.GetByEmail("test"))
                .Returns(user);

            AuthService.Setup(x => x.GenerateToken(user.Email, user.Role, user.Id))
                .Returns(new UserToken
                {
                    Token = "someTokenXYZ"
                });

            //Act
            var result = Logic.RenewToken("test");

            //Assert
            Assert.True(result.IsSuccessfull);
            Assert.Contains(result.Value.Token, "someTokenXYZ");
            Assert.Equal(0, (int)result.ErrorMessages.Count());
        }

        [Fact]
        public void RegisterWorkerReturnsErrorWhenDataIsNull()
        {
            //Arrange
            UserModel data = null;
            //Act
            var result = Logic.RegisterWokrer(data,"Worker");

            //Assert
            Assert.False(result.IsSuccessfull);
            Assert.Contains(result.ErrorMessages, item => item.Message == "Data was null");
        }

        [Fact]
        public void RegisterWorkerReturnsErrorWhenUserValidationFails()
        {
            //Arrange
            var user = new UserModel
            {
                Id = 1,
                Password = "zaq1WSX",
                Email = "worker@wp.pl",
                FirstName = "Admin",
                Surname = "Admin"
            };

            Validator.Setup(x => x.Validate(It.IsAny<ValidationContext<UserModel>>()).IsValid)
                .Returns(false);

            //Act
            var result = Logic.RegisterWokrer(user, "Worker");

            //Assert
            Assert.False(result.IsSuccessfull);
            Assert.Contains(result.ErrorMessages, item => item.Message == "Error during user data validation");
        }

        [Fact]
        public void RegisterWorkerReturnsErrorWhenHashedPasswordIsEmpty()
        {
            //Arrange
            var user = new UserModel
            {
                Id = 1,
                Password = "zaq1WSX",
                Email = "worker@wp.pl",
                FirstName = "Admin",
                Surname = "Admin"
            };

            Validator.Setup(x => x.Validate(It.IsAny<ValidationContext<UserModel>>()).IsValid)
                .Returns(true);

            AuthService.Setup(x => x.HashPassword(user.Password))
                .Returns("");

            //Act
            var result = Logic.RegisterWokrer(user,"Worker");

            //Assert
            Assert.False(result.IsSuccessfull);
            Assert.Contains(result.ErrorMessages, item => item.Message == "Error during password hashing");
        }

        [Fact]
        public void RegisterWorkerReturnsErrorWhenHashedPasswordIsNull()
        {
            //Arrange
            var user = new UserModel
            {
                Id = 1,
                Password = "zaq1WSX",
                Email = "worker@wp.pl",
                FirstName = "Admin",
                Surname = "Admin",
                Role="Worker"
            };

            Validator.Setup(x => x.Validate(It.IsAny<ValidationContext<UserModel>>()).IsValid)
                .Returns(true);

            AuthService.Setup(x => x.HashPassword(user.Password))
                .Returns((string)null);

            //Act
            var result = Logic.RegisterWokrer(user,"Worker");

            //Assert
            Assert.False(result.IsSuccessfull);
            Assert.Contains(result.ErrorMessages, item => item.Message == "Error during password hashing");
        }

        [Fact]
        public void RegisterWorkerReturnsErrorWhenExceptionOccurs()
        {
            //Arrange
            var user = new UserModel
            {
                Id = 1,
                Password = "zaq1WSX",
                Email = "worker@wp.pl",
                FirstName = "Admin",
                Surname = "Admin"
            };

            Validator.Setup(x => x.Validate(It.IsAny<ValidationContext<UserModel>>()).IsValid)
                .Returns(true);

            AuthService.Setup(x => x.HashPassword(user.Password))
                .Returns("Xl3FqDMyjnN7LnKWp2LbtR0r11OFXgXHRBZeHSZSq12Q+FWq");

            Repository.Setup(x => x.SaveChanges())
                .Throws(new UniqueConstraintException("Unique constraint violation",new Exception { 
                    
                }));

            //Act
            var result = Logic.RegisterWokrer(user, "Worker");

            //Assert
            
            Repository.Verify(x=>x.Add(user),Times.Exactly(1));
            Repository.Verify(x=>x.SaveChanges(),Times.Exactly(1));
            Assert.False(result.IsSuccessfull);
        }

        [Fact]
        public void RegisterWorkerReturnsOkWhenEverythingIsOk()
        {
            //Arrange
            var user = new UserModel
            {
                Id = 1,
                Password = "zaq1WSX",
                Email = "worker@wp.pl",
                FirstName = "Admin",
                Surname = "Admin"
            };

            Validator.Setup(x => x.Validate(It.IsAny<ValidationContext<UserModel>>()).IsValid)
                .Returns(true);

            AuthService.Setup(x => x.HashPassword(user.Password))
                .Returns("Xl3FqDMyjnN7LnKWp2LbtR0r11OFXgXHRBZeHSZSq12Q+FWq");

            Repository.Setup(x => x.SaveChanges());

            //Act
            var result = Logic.RegisterWokrer(user,"Worker");

            //Assert

            Repository.Verify(x => x.Add(user), Times.Exactly(1));
            Repository.Verify(x => x.SaveChanges(), Times.Exactly(1));
            Assert.True(result.IsSuccessfull);
        }

        [Fact]
        public void RegisterAccountReturnsErrorWhenEmailIsBad()
        {
            //Arrange

            //Act
            var result = Logic.RegisterAccount("", "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJlbWFpbCI6InN6eW1vbktvd2Fsc2tpQHdwLnBsIiwiUm9sZSI6IldvcmtlciIsIlVzZXJJZCI6IjEiLCJuYmYiOjE2MzUyODgzNjgsImV4cCI6MTYzNTI5MTk2OCwiaWF0IjoxNjM1Mjg4MzY4fQ.cI0bC9p_-edBiDpO765KYN64zS2h0cuLognmnB3Myqk", "dsa");


            //Assert
            Assert.False(result.IsSuccessfull);
            Assert.Contains(result.ErrorMessages, item => item.Message == "Error during validation");
        }

        [Fact]
        public void RegisterAccountReturnsErrorWhenTokenEmailIsEmpty()
        {
            //Arrange

            //Act
            var result = Logic.RegisterAccount("szymonkowalski@wp.pl", "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJlbWFpbCI6IiIsInN1YiI6IjEyMzQ1Njc4OTAiLCJuYW1lIjoiSm9obiBEb2UiLCJpYXQiOjE1MTYyMzkwMjJ9.OVASBAZm9NcwZynD1Ad9-vhQFiDeJ9rqhFs4jlo4-JU", "zaq1@WSX");

            //Assert
            Assert.False(result.IsSuccessfull);
            Assert.Contains(result.ErrorMessages, item => item.Message == "Error during validation");
        }

        [Fact]
        public void RegisterAccountReturnsErrorWhenNoUserIsFound()
        {
            //Arrange

            //Act
            var result = Logic.RegisterAccount("szymonKowalski@wp.pl", "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJlbWFpbCI6InN6eW1vbktvd2Fsc2tpQHdwLnBsIiwiUm9sZSI6IldvcmtlciIsIlVzZXJJZCI6IjEiLCJuYmYiOjE2MzUyODgzNjgsImV4cCI6MTYzNTI5MTk2OCwiaWF0IjoxNjM1Mjg4MzY4fQ.cI0bC9p_-edBiDpO765KYN64zS2h0cuLognmnB3Myqk", "szymonKowalski@wp.pl");

            //Assert
            Assert.False(result.IsSuccessfull);
            Assert.Contains(result.ErrorMessages, item => item.Message == "Error during validation");
        }

        [Fact]
        public void RegisterAccountReturnsErrorWhenPasswordIsInvalid()
        {
            //Arrange
            var user = new UserModel
            {
                Email = "szymonKowalski@wp.pl",
                Password = null,
            };

            Repository.Setup(x => x.GetByEmail("szymonKowalski@wp.pl"))
                .Returns(user);

            Validator.Setup(x => x.Validate(It.IsAny<ValidationContext<UserModel>>()).IsValid)
                .Returns(false);

            //Act
            var result = Logic.RegisterAccount("szymonKowalski@wp.pl", "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJlbWFpbCI6InN6eW1vbktvd2Fsc2tpQHdwLnBsIiwiUm9sZSI6IldvcmtlciIsIlVzZXJJZCI6IjEiLCJuYmYiOjE2MzUyODgzNjgsImV4cCI6MTYzNTI5MTk2OCwiaWF0IjoxNjM1Mjg4MzY4fQ.cI0bC9p_-edBiDpO765KYN64zS2h0cuLognmnB3Myqk", "szymonKowalski@wp.pl");

            //Assert
            Assert.False(result.IsSuccessfull);
            Assert.Contains(result.ErrorMessages, item => item.Message == "Error during password validationn");
        }

        [Fact]
        public void RegisterAccountReturnsErrorWhenHashedPasswordIsEmpty()
        {
            //Arrange
            var user = new UserModel
            {
                Email = "szymonKowalski@wp.pl",
                Password = null,
            };

            Repository.Setup(x => x.GetByEmail("szymonKowalski@wp.pl"))
                .Returns(user);

            Validator.Setup(x => x.Validate(It.IsAny<ValidationContext<UserModel>>()).IsValid)
                .Returns(true);

            AuthService.Setup(x => x.HashPassword("szymonKowalski@wp.pl"))
                .Returns("");

            //Act
            var result = Logic.RegisterAccount("szymonKowalski@wp.pl", "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJlbWFpbCI6InN6eW1vbktvd2Fsc2tpQHdwLnBsIiwiUm9sZSI6IldvcmtlciIsIlVzZXJJZCI6IjEiLCJuYmYiOjE2MzUyODgzNjgsImV4cCI6MTYzNTI5MTk2OCwiaWF0IjoxNjM1Mjg4MzY4fQ.cI0bC9p_-edBiDpO765KYN64zS2h0cuLognmnB3Myqk", "szymonKowalski@wp.pl");

            //Assert
            Assert.False(result.IsSuccessfull);
            Assert.Contains(result.ErrorMessages, item => item.Message == "Error during email hashing");
        }

        [Fact]
        public void RegisterAccountReturnsErrorWhenHashedPasswordIsNull()
        {
            //Arrange
            var user = new UserModel
            {
                Email = "szymonKowalski@wp.pl",
                Password = null,
            };

            Repository.Setup(x => x.GetByEmail("szymonKowalski@wp.pl"))
                .Returns(user);

            Validator.Setup(x => x.Validate(It.IsAny<ValidationContext<UserModel>>()).IsValid)
                .Returns(true);

            AuthService.Setup(x => x.HashPassword("szymonKowalski@wp.pl"))
                .Returns((string)null);

            //Act
            var result = Logic.RegisterAccount("szymonKowalski@wp.pl", "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJlbWFpbCI6InN6eW1vbktvd2Fsc2tpQHdwLnBsIiwiUm9sZSI6IldvcmtlciIsIlVzZXJJZCI6IjEiLCJuYmYiOjE2MzUyODgzNjgsImV4cCI6MTYzNTI5MTk2OCwiaWF0IjoxNjM1Mjg4MzY4fQ.cI0bC9p_-edBiDpO765KYN64zS2h0cuLognmnB3Myqk", "szymonKowalski@wp.pl");

            //Assert
            Assert.False(result.IsSuccessfull);
            Assert.Contains(result.ErrorMessages, item => item.Message == "Error during email hashing");
        }

        [Fact]
        public void RegisterAccountThrowsError()
        {
            //Arrange
            var user = new UserModel
            {
                Email = "szymonKowalski@wp.pl",
                Password = null,
            };

            Repository.Setup(x => x.GetByEmail("szymonKowalski@wp.pl"))
                .Returns(user);

            Validator.Setup(x => x.Validate(It.IsAny<ValidationContext<UserModel>>()).IsValid)
                .Returns(true);

            AuthService.Setup(x => x.HashPassword("szymonKowalski@wp.pl"))
                .Returns("hey");

            Repository.Setup(x => x.SaveChanges())
                .Throws(new UniqueConstraintException("Unique constraint violation", new Exception
                {

                }));

            //Act
            var result = Logic.RegisterAccount("szymonKowalski@wp.pl", "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJlbWFpbCI6InN6eW1vbktvd2Fsc2tpQHdwLnBsIiwiUm9sZSI6IldvcmtlciIsIlVzZXJJZCI6IjEiLCJuYmYiOjE2MzUyODgzNjgsImV4cCI6MTYzNTI5MTk2OCwiaWF0IjoxNjM1Mjg4MzY4fQ.cI0bC9p_-edBiDpO765KYN64zS2h0cuLognmnB3Myqk", "szymonKowalski@wp.pl");

            //Assert
            Repository.Verify(x => x.SaveChanges(), Times.Exactly(1));
            Assert.False(result.IsSuccessfull);
        }

        [Fact]
        public void RegisterAccountRegistersWhenEverythingIsOk()
        {
            //Arrange
            var user = new UserModel
            {
                Email = "szymonKowalski@wp.pl",
                Password = null,
            };

            Repository.Setup(x => x.GetByEmail("szymonKowalski@wp.pl"))
                .Returns(user);

            Validator.Setup(x => x.Validate(It.IsAny<ValidationContext<UserModel>>()).IsValid)
                .Returns(true);

            AuthService.Setup(x => x.HashPassword("szymonKowalski@wp.pl"))
                .Returns("hey");

            //Act
            var result = Logic.RegisterAccount("szymonKowalski@wp.pl", "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJlbWFpbCI6InN6eW1vbktvd2Fsc2tpQHdwLnBsIiwiUm9sZSI6IldvcmtlciIsIlVzZXJJZCI6IjEiLCJuYmYiOjE2MzUyODgzNjgsImV4cCI6MTYzNTI5MTk2OCwiaWF0IjoxNjM1Mjg4MzY4fQ.cI0bC9p_-edBiDpO765KYN64zS2h0cuLognmnB3Myqk", "szymonKowalski@wp.pl");

            //Assert
            Repository.Verify(x => x.SaveChanges(), Times.Exactly(1));
            Assert.True(result.IsSuccessfull);
        }
    }
}
