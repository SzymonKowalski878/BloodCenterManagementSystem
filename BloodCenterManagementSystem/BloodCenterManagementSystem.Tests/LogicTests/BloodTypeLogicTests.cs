using BloodCenterManagementSystem.Logics;
using BloodCenterManagementSystem.Logics.BloodTypes;
using BloodCenterManagementSystem.Logics.Repositories;
using BloodCenterManagementSystem.Models;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Xunit;

namespace BloodCenterManagementSystem.Tests.LogicTests
{
    public class BloodTypeLogicTests
    {
        private readonly BloodTypeLogic Logic;
        private readonly Mock<IBloodTypeRepository> Repository = new Mock<IBloodTypeRepository>();

        public BloodTypeLogicTests()
        {
            var LazyRepository = new Lazy<IBloodTypeRepository>(() => Repository.Object);
            Logic = new BloodTypeLogic(LazyRepository);
        }

        [Fact]
        public void GetByIdReturnsBloodTypeWhenItExists()
        {
            //Arrange
            var bloodType = new BloodTypeModel
            {
                Id = 1,
                BloodTypeName = "0Rh+",
                AmmountOfBloodInBank = 0
            };

            Repository.Setup(x => x.GetById(1))
                .Returns(bloodType);

            //Act
            var result = Logic.GetById(1);

            //Assert
            Assert.Equal(bloodType.Id, result.Value.Id);
            Assert.Equal(bloodType.BloodTypeName, result.Value.BloodTypeName);
            Assert.Equal(bloodType.AmmountOfBloodInBank, result.Value.AmmountOfBloodInBank);
            Assert.True(result.IsSuccessfull);
            Assert.NotNull(result.ErrorMessages);
        }

        [Fact]
        public void GetByIdReturnsErrorWhenNoUserIsFound()
        {
            //Arrange
            Repository.Setup(x => x.GetById(1))
                .Returns((BloodTypeModel)null);

            //Act
            var result = Logic.GetById(1);

            //Assert
            Assert.Null(result.Value);
        }

        [Fact]
        public void GetByNameReturnsErrorWhenBloodTypeNameIsNull()
        {
            //Act
            var result = Logic.GetByName(null);

            //Assert
            Assert.False(result.IsSuccessfull);
            Assert.Null(null);
            Assert.Contains(result.ErrorMessages,item=>item.Message== "Data was null");
        }

        [Fact]
        public void GetByNameReturnsErrorWhenBloodTypeNameIsEmpty()
        {
            //Act
            var result = Logic.GetByName("");

            //Assert
            Assert.False(result.IsSuccessfull);
            Assert.Empty("");
            Assert.Contains(result.ErrorMessages, item => item.Message == "Data was null");
        }

        [Fact]
        public void GetByNameReturnsErrorWhenNoBloodTypeFound()
        {
            //Arrange
            Repository.Setup(x => x.GetByBloodTypeName("0rh+"))
                .Returns((BloodTypeModel)null);

            //Act
            var result = Logic.GetByName("0rh+");

            //Assert
            Assert.Null(result.Value);
        }

        [Fact]
        public void GetByNameReturnsErrorWhenBloodTypeExists()
        {
            //Arrange
            var bloodType = new BloodTypeModel
            {
                Id = 1,
                BloodTypeName = "0Rh+",
                AmmountOfBloodInBank = 0
            };

            Repository.Setup(x => x.GetByBloodTypeName("0Rh+"))
                .Returns(bloodType);

            //Act
            var result = Logic.GetByName("0Rh+");

            //Assert
            Assert.Equal(bloodType.Id, result.Value.Id);
            Assert.Equal(bloodType.BloodTypeName, result.Value.BloodTypeName);
            Assert.Equal(bloodType.AmmountOfBloodInBank, result.Value.AmmountOfBloodInBank);
            Assert.True(result.IsSuccessfull);
            Assert.NotNull(result.ErrorMessages);
        }
        
        [Fact]
        public void GetAllBloodTypesReturnsErrorWhenNotAllRecordsWereFound()
        {
            //Arrange
            var bloodTypes = new List<BloodTypeModel>() {
                new BloodTypeModel{
                    Id=1,
                    BloodTypeName="0Rh+",
                    AmmountOfBloodInBank=0
                },
                new BloodTypeModel{
                    Id=2,
                    BloodTypeName="0Rh-",
                    AmmountOfBloodInBank=0
                },
                new BloodTypeModel{
                    Id=3,
                    BloodTypeName="0Rh+",
                    AmmountOfBloodInBank=0
                },
                new BloodTypeModel{
                    Id=4,
                    BloodTypeName="0Rh+",
                    AmmountOfBloodInBank=0
                }
            };


            Repository.Setup(x => x.GetAll())
                .Returns(bloodTypes);

            //Act

            var result = Logic.GetAllBloodTypes();

            //Assert
            Assert.False(result.IsSuccessfull);
        }

        [Fact]
        public void GetAllBloodTypesReturnsAllData()
        {
            //Arrange
            var bloodTypes = new List<BloodTypeModel>() {
                new BloodTypeModel{
                    Id=1,
                    BloodTypeName="0Rh+",
                    AmmountOfBloodInBank=0
                },
                new BloodTypeModel{
                    Id=2,
                    BloodTypeName="0Rh-",
                    AmmountOfBloodInBank=0
                },
                new BloodTypeModel{
                    Id=3,
                    BloodTypeName="ARh+",
                    AmmountOfBloodInBank=0
                },
                new BloodTypeModel{
                    Id=4,
                    BloodTypeName="ARh-",
                    AmmountOfBloodInBank=0
                },
                new BloodTypeModel{
                    Id=5,
                    BloodTypeName="BRh+",
                    AmmountOfBloodInBank=0
                },
                new BloodTypeModel{
                    Id=6,
                    BloodTypeName="BRh-",
                    AmmountOfBloodInBank=0
                },
                new BloodTypeModel{
                    Id=7,
                    BloodTypeName="ABRh+",
                    AmmountOfBloodInBank=0
                },
                new BloodTypeModel{
                    Id=8,
                    BloodTypeName="ABRh-",
                    AmmountOfBloodInBank=0
                },
            };

            Repository.Setup(x => x.GetAll())
                .Returns(bloodTypes);

            //Act

            var result = Logic.GetAllBloodTypes();

            //Assert
            Assert.True(result.IsSuccessfull);
            Assert.Equal(8, result.Value.Count());
        }
    }
}
