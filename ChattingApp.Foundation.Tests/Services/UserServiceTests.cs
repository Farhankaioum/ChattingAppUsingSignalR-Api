using Autofac.Extras.Moq;
using ChattingApp.Foundation.Entities;
using ChattingApp.Foundation.Helpers;
using ChattingApp.Foundation.Repositories;
using ChattingApp.Foundation.Services;
using ChattingApp.Foundation.UnitOfWorks;
using Moq;
using NUnit.Framework;
using Shouldly;
using System;

namespace ChattingApp.Foundation.Tests.Services
{
    public class UserServiceTests
    {
        private AutoMock _mock;
        private Mock<IChattingUnitOfWork> _chattingUnitOfWorkMock;
        private Mock<IUserRepository> _userRepositoryMock;
        private Mock<IValidator> _validatorMock;
        private Mock<IUserService> _userServiceMock;
        private IUserService _userService;

        [OneTimeSetUp]
        public void OneTimeSetUp()
        {
            _mock = AutoMock.GetLoose();
        }

        [SetUp]
        public void SetUp()
        {
            _chattingUnitOfWorkMock = _mock.Mock<IChattingUnitOfWork>();
            _userRepositoryMock = _mock.Mock<IUserRepository>();
            _validatorMock = _mock.Mock<IValidator>();
            _userServiceMock = _mock.Mock<IUserService>();
            _userService = _mock.Create<UserService>();
        }

        [Test, Category("Unit Test")]
        public void AddUser_FirstNameEmpty_ThrowEmptyException()
        {
            // Arrange
            var user = new User
            {
                LastName = "Yyy",
                Email = "xy@gmail.com"
            };

            _chattingUnitOfWorkMock.Setup(x => x.UserRepository)
                .Returns(_userRepositoryMock.Object);

            //_validatorMock.Setup(x => x.IsValidString(user.FirstName))
            //    .Returns(false);

            // Act
            //_userService.AddUser(user);

            // Assert
            Assert.That(() => _userService.AddUser(user), Throws.Exception.TypeOf<EmptyException>());
        }

        [Test, Category("Unit Test")]
        public void AddUser_UserProvided_RegisterUser()
        {   
            // Arrange
            var user = new User { 
                FirstName = "xyx",
                LastName = "Yyy",
                Email = "xy@gmail.com"
            };

            _chattingUnitOfWorkMock.Setup(x => x.UserRepository)
                .Returns(_userRepositoryMock.Object);

            _validatorMock.Setup(x => x.IsValidString(user.FirstName))
                .Returns(true);
            _validatorMock.Setup(x => x.IsValidString(user.LastName))
                .Returns(true);
            _validatorMock.Setup(x => x.IsValidEmail(user.Email))
                .Returns(true);
            _validatorMock.Setup(x => x.IsEmailDuplicateInDB(user.Email))
                .Returns(false);

            _userRepositoryMock.Setup(x => x.Add(user)).Verifiable();

            _chattingUnitOfWorkMock.Setup(x => x.Save()).Verifiable();

            // Act
            _userService.AddUser(user);

            // Assert
            this.ShouldSatisfyAllConditions(
                () => _chattingUnitOfWorkMock.VerifyAll(),
                () => _userRepositoryMock.VerifyAll()
                );
        }

        [Test, Category("Unit Test")]
        public void GetUserById_NonExistingId_ThrowNullReferenceException()
        {
            // Arrange
            var user = new User
            {
                Id = Guid.NewGuid(),
                FirstName = "xyx",
                LastName = "Yyy",
                Email = "xy@gmail.com"
            };

            _chattingUnitOfWorkMock.Setup(x => x.UserRepository)
                .Returns(_userRepositoryMock.Object);

            //_userServiceMock.Setup(x => x.GetUserById(user.Id))
            //    .Throws(new NullReferenceException());

            // Act
            //var result = _userService.GetUserById(user.Id);

            // Assert
            //Assert.That(() => _userService.GetUserById(user.Id), Throws.Exception.TypeOf<NullReferenceException>());
            Should.Throw<NullReferenceException>(() => {
                _userService.GetUserById(user.Id);
            });

        }

        [TearDown]
        public void Clean()
        {
            _chattingUnitOfWorkMock?.Reset();
        }

        [OneTimeTearDown]
        public void OneTimeClean()
        {
            _mock?.Dispose();
        }
    }
}
