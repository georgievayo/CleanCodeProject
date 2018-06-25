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
        private Mock<IRepository<User>> usersRepositoryMock;
        private Mock<IRepository<Manager>> managersRepositoryMock;
        private Mock<IUnitOfWork> unitOfWorkMock;
        private Mock<IUsersFactory> usersFactoryMock;
        private Mock<IManagersFactory> managersFactoryMock;
        private UsersService service;

        [TestCase("pesho", "pass", "pesho@gmail.com", "Pesho", "Peshov", "0863566565", "user")]
        public void MethodCreateShould_CallFactoryMethodCreate(string username, string password, 
            string email, string firstName, string lastName, string phoneNumber, string role)
        {
            service.Create(username, password, email, firstName, lastName, phoneNumber, role);

            usersFactoryMock.Verify(f => f.Create(username, password, email, 
                firstName, lastName, phoneNumber), Times.Once);
        }

        [TestCase("pesho", "pass", "pesho@gmail.com", "Pesho", "Peshov", "0863566565", "user")]
        public void MethodCreateShould_ReturnCorrectUser(string username, string password, 
            string email, string firstName, string lastName, string phoneNumber, string role)
        {
            var user = new User(username, password, email, firstName, lastName, phoneNumber);
            usersFactoryMock.Setup(f => f.Create(username, password, email, firstName, lastName, phoneNumber))
                .Returns(user);

            var result = service.Create(username, password, email, firstName, lastName, phoneNumber, role);

            Assert.AreSame(user, result);
        }

        [TestCase("d547a40d-c45f-4c43-99de-0bfe9199ff95")]
        [TestCase("99ae8dd3-1067-4141-9675-62e94bb6caaa")]
        public void MethodDeleteShould_CallRepositoryMethodGetById(string id)
        {
            var guidId = Guid.Parse(id);

            service.Delete(guidId);

            usersRepositoryMock.Verify(r => r.GetById(guidId), Times.Once);
        }

        [TestCase("d547a40d-c45f-4c43-99de-0bfe9199ff95")]
        [TestCase("99ae8dd3-1067-4141-9675-62e94bb6caaa")]
        public void MethodDeleteShould_CallRepositoryMethodDelete(string id)
        {
            var guidId = Guid.Parse(id);
            var user = new User() { Id = guidId };

            usersRepositoryMock.Setup(r => r.GetById(guidId))
                .Returns(user);

            service.Delete(guidId);

            usersRepositoryMock.Verify(r => r.Delete(user), Times.Once);
        }

        [TestCase("d547a40d-c45f-4c43-99de-0bfe9199ff95")]
        [TestCase("99ae8dd3-1067-4141-9675-62e94bb6caaa")]
        public void MethodDeleteShould_CallUnitOfWorkMethodCommit(string id)
        {
            var guidId = Guid.Parse(id);
            usersRepositoryMock.Setup(r => r.GetById(guidId))
                .Returns(() => new User() { Id = guidId });

           service.Delete(guidId);

            unitOfWorkMock.Verify(r => r.Commit(), Times.Once);
        }

        [TestCase("d547a40d-c45f-4c43-99de-0bfe9199ff95")]
        [TestCase("99ae8dd3-1067-4141-9675-62e94bb6caaa")]
        public void MethodGetByIdShould_CallRepositoryMethodGetById(string id)
        {
            var guidId = Guid.Parse(id);

            service.GetById(guidId);

            usersRepositoryMock.Verify(r => r.GetById(guidId), Times.Once);
        }

        [TestCase("d547a40d-c45f-4c43-99de-0bfe9199ff95")]
        [TestCase("99ae8dd3-1067-4141-9675-62e94bb6caaa")]
        public void MethodGetByIdShould_ReturnCorrectUser(string id)
        {
            var guidId = Guid.Parse(id);
            var user = new User()
            {
                Id = guidId
            };

            usersRepositoryMock.Setup(r => r.GetById(guidId))
                .Returns(user);

            var result = service.GetById(guidId);

            Assert.AreSame(user, result);
        }

        [TestCase("user1")]
        [TestCase("user2")]
        public void MethodGetByUsernameShould_CallRepositoryPropertyAll(string username)
        {
            service.GetByUsername(username);

            usersRepositoryMock.Verify(r => r.All, Times.Once);
        }

        [TestCase("user1")]
        [TestCase("user2")]
        public void MethodGetByUsernameShould_ReturnCorrectUser(string username)
        {
            var foundUser = new User() { UserName = username };

            usersRepositoryMock.Setup(r => r.All)
                .Returns(new List<User> { foundUser }.AsQueryable());

            var result = service.GetByUsername(username);

            Assert.AreSame(foundUser, result);
        }

        public void MethodGetUserShould_CallRepositoryPropertyAll()
        {
            var userId = Guid.NewGuid();

            service.GetUser(userId);

            usersRepositoryMock.Verify(r => r.All, Times.Once);
        }

        public void MethodGetUserShould_ReturnCorrectUserWithBookings(Guid id)
        {
            var booking = new Booking() { Id = Guid.NewGuid() };
            var user = new User() { Id = id, Bookings = new List<Booking>() { booking } };

            usersRepositoryMock.Setup(r => r.All)
                .Returns(new List<User> { user }.AsQueryable<User>);

            var result = service.GetUser(id);

            Assert.AreSame(user, result);
        }

        [SetUp]
        public void SetUp()
        {
            usersRepositoryMock = new Mock<IRepository<User>>();
            managersRepositoryMock = new Mock<IRepository<Manager>>();
            unitOfWorkMock = new Mock<IUnitOfWork>();
            usersFactoryMock = new Mock<IUsersFactory>();
            managersFactoryMock = new Mock<IManagersFactory>();

            service = new UsersService(usersRepositoryMock.Object, managersRepositoryMock.Object, 
                unitOfWorkMock.Object, usersFactoryMock.Object, managersFactoryMock.Object);
        }
    }
}
