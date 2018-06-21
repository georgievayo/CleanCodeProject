using FindAndBook.Data.Contracts;
using FindAndBook.Factories;
using FindAndBook.Models;
using FindAndBook.Services;
using Moq;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;

namespace FindAndBook.Tests.Services
{
    [TestFixture]
    public class UsersServiceTests
    {
        private Mock<IRepository<User>> repositoryMock;
        private Mock<IUnitOfWork> unitOfWorkMock;
        private Mock<IUsersFactory> usersFactoryMock;
        private Mock<IManagersFactory> managersFactoryMock;

        [TestCase("pesho", "pass", "pesho@gmail.com", "Pesho", "Peshov", "0863566565", "user")]
        public void MethodCreateShould_CallFactoryMethodCreate(string username, string password, string email, string firstName, string lastName, string phoneNumber, string role)
        {
            var service = new UsersService(repositoryMock.Object, unitOfWorkMock.Object, usersFactoryMock.Object, managersFactoryMock.Object);
            service.Create(username, password, email, firstName, lastName, phoneNumber, role);

            usersFactoryMock.Verify(f => f.Create(username, password, email, firstName, lastName, phoneNumber), Times.Once);
        }

        [TestCase("pesho", "pass", "pesho@gmail.com", "Pesho", "Peshov", "0863566565", "user")]
        public void MethodCreateShould_ReturnCorrectUser(string username, string password, string email, string firstName, string lastName, string phoneNumber, string role)
        {
            var user = new User(username, password, email, firstName, lastName, phoneNumber);
            usersFactoryMock.Setup(f => f.Create(username, password, email, firstName, lastName, phoneNumber)).Returns(user);

            var service = new UsersService(repositoryMock.Object, unitOfWorkMock.Object, usersFactoryMock.Object, managersFactoryMock.Object);
            var result = service.Create(username, password, email, firstName, lastName, phoneNumber, role);

            Assert.AreSame(user, result);
        }

        [TestCase("d547a40d-c45f-4c43-99de-0bfe9199ff95")]
        [TestCase("99ae8dd3-1067-4141-9675-62e94bb6caaa")]
        public void MethodDeleteShould_CallRepositoryMethodGetById(string id)
        {
            var guidId = Guid.Parse(id);

            var service = new UsersService(repositoryMock.Object, unitOfWorkMock.Object, usersFactoryMock.Object, managersFactoryMock.Object);
            service.Delete(guidId);

            repositoryMock.Verify(r => r.GetById(guidId), Times.Once);
        }

        [TestCase("d547a40d-c45f-4c43-99de-0bfe9199ff95")]
        [TestCase("99ae8dd3-1067-4141-9675-62e94bb6caaa")]
        public void MethodDeleteShould_CallRepositoryMethodDelete(string id)
        {
            var guidId = Guid.Parse(id);
            var user = new User() { Id = guidId };
            repositoryMock.Setup(r => r.GetById(guidId)).Returns(user);

            var service = new UsersService(repositoryMock.Object, unitOfWorkMock.Object, usersFactoryMock.Object, managersFactoryMock.Object);
            service.Delete(guidId);

            repositoryMock.Verify(r => r.Delete(user), Times.Once);
        }

        [TestCase("d547a40d-c45f-4c43-99de-0bfe9199ff95")]
        [TestCase("99ae8dd3-1067-4141-9675-62e94bb6caaa")]
        public void MethodDeleteShould_CallUnitOfWorkMethodCommit(string id)
        {
            var guidId = Guid.Parse(id);
            repositoryMock.Setup(r => r.GetById(guidId)).Returns(() => new User() { Id = guidId });
          
            var service = new UsersService(repositoryMock.Object, unitOfWorkMock.Object, usersFactoryMock.Object, managersFactoryMock.Object);
            service.Delete(guidId);

            unitOfWorkMock.Verify(r => r.Commit(), Times.Once);
        }

        [Test]
        public void MethodGetAllShould_CallRepositoryPropertyAll()
        {
            var service = new UsersService(repositoryMock.Object, unitOfWorkMock.Object, usersFactoryMock.Object, managersFactoryMock.Object);
            service.GetAll();

            repositoryMock.Verify(r => r.All, Times.Once);
        }

        [Test]
        public void MethodGetAllShould_ReturnCorrectResult()
        {
            var user = new User() { UserName = "user1" };

            repositoryMock.Setup(r => r.All).Returns(new List<User> { user }.AsQueryable<User>);
            var service = new UsersService(repositoryMock.Object, unitOfWorkMock.Object, usersFactoryMock.Object, managersFactoryMock.Object);
            var result = service.GetAll();

            Assert.Contains(user, result.ToList());
        }

        [TestCase("d547a40d-c45f-4c43-99de-0bfe9199ff95")]
        [TestCase("99ae8dd3-1067-4141-9675-62e94bb6caaa")]
        public void MethodGetByIdShould_CallRepositoryMethodGetById(string id)
        {
            var guidId = Guid.Parse(id);

            var service = new UsersService(repositoryMock.Object, unitOfWorkMock.Object, usersFactoryMock.Object, managersFactoryMock.Object);
            service.GetById(guidId);

            repositoryMock.Verify(r => r.GetById(guidId), Times.Once);
        }

        [TestCase("d547a40d-c45f-4c43-99de-0bfe9199ff95")]
        [TestCase("99ae8dd3-1067-4141-9675-62e94bb6caaa")]
        public void MethodGetByIdShould_ReturnCorrectUser(string id)
        {
            var foundUserMock = new Mock<User>();
            var guidId = Guid.Parse(id);

            repositoryMock.Setup(r => r.GetById(guidId)).Returns(foundUserMock.Object);

            var service = new UsersService(repositoryMock.Object, unitOfWorkMock.Object, usersFactoryMock.Object, managersFactoryMock.Object);
            var result = service.GetById(guidId);

            Assert.AreSame(foundUserMock.Object, result);
        }

        [TestCase("user1")]
        [TestCase("user2")]
        public void MethodGetByUsernameShould_CallRepositoryPropertyAll(string username)
        {
            var service = new UsersService(repositoryMock.Object, unitOfWorkMock.Object, usersFactoryMock.Object, managersFactoryMock.Object);
            service.GetByUsername(username);

            repositoryMock.Verify(r => r.All, Times.Once);
        }

        [TestCase("user1")]
        [TestCase("user2")]
        public void MethodGetByUsernameShould_ReturnCorrectUser(string username)
        {
            var foundUser = new User() { UserName = username };

            repositoryMock.Setup(r => r.All).Returns(new List<User> { foundUser }.AsQueryable());

            var service = new UsersService(repositoryMock.Object, unitOfWorkMock.Object, usersFactoryMock.Object, managersFactoryMock.Object);
            var result = service.GetByUsername(username);

            Assert.AreSame(foundUser, result);
        }

        [TestCase("d547a40d-c45f-4c43-99de-0bfe9199ff95")]
        [TestCase("99ae8dd3-1067-4141-9675-62e94bb6caaa")]
        public void MethodGetUserWithBookingsShould_CallRepositoryPropertyAll(string id)
        {
            var guidId = Guid.Parse(id);

            var service = new UsersService(repositoryMock.Object, unitOfWorkMock.Object, usersFactoryMock.Object, managersFactoryMock.Object);
            service.GetUserWithBookings(guidId);

            repositoryMock.Verify(r => r.All, Times.Once);
        }

        [TestCase("d547a40d-c45f-4c43-99de-0bfe9199ff95")]
        [TestCase("99ae8dd3-1067-4141-9675-62e94bb6caaa")]
        public void MethodGetUserWithBookingsShould_ReturnCorrectUserWithBookings(string id)
        {
            var booking = new Booking() { Id = new Guid("d547a40d-c45f-4c43-99de-0bfe9199ff95") };
            var guidId = Guid.Parse(id);
            var user = new User() { Id = guidId, Bookings = new List<Booking>() { booking } };

            repositoryMock.Setup(r => r.All).Returns(new List<User> { user }.AsQueryable<User>);
            var service = new UsersService(repositoryMock.Object, unitOfWorkMock.Object, usersFactoryMock.Object, managersFactoryMock.Object);
            var result = service.GetUserWithBookings(guidId);

            Assert.AreSame(user, result);
        }

        [SetUp]
        public void SetUp()
        {
            repositoryMock = new Mock<IRepository<User>>();
            unitOfWorkMock = new Mock<IUnitOfWork>();
            usersFactoryMock = new Mock<IUsersFactory>();
            managersFactoryMock = new Mock<IManagersFactory>();
        }
    }
}
