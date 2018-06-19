﻿using FindAndBook.Data.Contracts;
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
        [TestCase("pesho", "pesho@gmail.com", "Pesho", "Peshov", "0863566565")]
        public void MethodCreateShould_CallFactoryMethodCreate(string username, string email, string firstName, string lastName, string phoneNumber)
        {
            var repositoryMock = new Mock<IRepository<User>>();
            var unitOfWorkMock = new Mock<IUnitOfWork>();
            var factoryMock = new Mock<IUsersFactory>();

            var service = new UsersService(repositoryMock.Object, unitOfWorkMock.Object, factoryMock.Object);
            service.Create(username, email, firstName, lastName, phoneNumber);

            factoryMock.Verify(f => f.Create(username, email, firstName, lastName, phoneNumber), Times.Once);
        }

        [TestCase("pesho", "pesho@gmail.com", "Pesho", "Peshov", "0863566565")]
        public void MethodCreateShould_ReturnCorrectUser(string username, string email, string firstName, string lastName, string phoneNumber)
        {
            var repositoryMock = new Mock<IRepository<User>>();
            var unitOfWorkMock = new Mock<IUnitOfWork>();
            var factoryMock = new Mock<IUsersFactory>();
            var user = new User(username, email, firstName, lastName, phoneNumber);

            factoryMock.Setup(f => f.Create(username, email, firstName, lastName, phoneNumber)).Returns(user);

            var service = new UsersService(repositoryMock.Object, unitOfWorkMock.Object, factoryMock.Object);
            var result = service.Create(username, email, firstName, lastName, phoneNumber);

            Assert.AreSame(user, result);
        }

        [TestCase("d547a40d-c45f-4c43-99de-0bfe9199ff95")]
        [TestCase("99ae8dd3-1067-4141-9675-62e94bb6caaa")]
        public void MethodDeleteShould_CallRepositoryMethodGetById(string id)
        {
            var repositoryMock = new Mock<IRepository<User>>();
            var unitOfWorkMock = new Mock<IUnitOfWork>();
            var factoryMock = new Mock<IUsersFactory>();

            var service = new UsersService(repositoryMock.Object, unitOfWorkMock.Object, factoryMock.Object);
            service.Delete(id);

            repositoryMock.Verify(r => r.GetById(id), Times.Once);
        }

        [TestCase("d547a40d-c45f-4c43-99de-0bfe9199ff95")]
        [TestCase("99ae8dd3-1067-4141-9675-62e94bb6caaa")]
        public void MethodDeleteShould_CallRepositoryMethodDelete(string id)
        {
            var repositoryMock = new Mock<IRepository<User>>();
            var unitOfWorkMock = new Mock<IUnitOfWork>();
            var factoryMock = new Mock<IUsersFactory>();

            var user = new User() { Id = id };
            repositoryMock.Setup(r => r.GetById(id)).Returns(user);

            var service = new UsersService(repositoryMock.Object, unitOfWorkMock.Object, factoryMock.Object);
            service.Delete(id);

            repositoryMock.Verify(r => r.Delete(user), Times.Once);
        }

        [TestCase("d547a40d-c45f-4c43-99de-0bfe9199ff95")]
        [TestCase("99ae8dd3-1067-4141-9675-62e94bb6caaa")]
        public void MethodDeleteShould_CallUnitOfWorkMethodCommit(string id)
        {
            var repositoryMock = new Mock<IRepository<User>>();
            var unitOfWorkMock = new Mock<IUnitOfWork>();
            var factoryMock = new Mock<IUsersFactory>();

            var service = new UsersService(repositoryMock.Object, unitOfWorkMock.Object, factoryMock.Object);
            service.Delete(id);

            unitOfWorkMock.Verify(r => r.Commit(), Times.Once);
        }

        [Test]
        public void MethodGetAllShould_CallRepositoryPropertyAll()
        {
            var repositoryMock = new Mock<IRepository<User>>();
            var unitOfWorkMock = new Mock<IUnitOfWork>();
            var factoryMock = new Mock<IUsersFactory>();

            var service = new UsersService(repositoryMock.Object, unitOfWorkMock.Object, factoryMock.Object);
            service.GetAll();

            repositoryMock.Verify(r => r.All, Times.Once);
        }

        [Test]
        public void MethodGetAllShould_ReturnCorrectResult()
        {
            var repositoryMock = new Mock<IRepository<User>>();
            var unitOfWorkMock = new Mock<IUnitOfWork>();
            var factoryMock = new Mock<IUsersFactory>();
            var user = new User() { UserName = "user1" };

            repositoryMock.Setup(r => r.All).Returns(new List<User> { user }.AsQueryable<User>);
            var service = new UsersService(repositoryMock.Object, unitOfWorkMock.Object, factoryMock.Object);
            var result = service.GetAll();

            Assert.Contains(user, result.ToList());
        }

        [TestCase("d547a40d-c45f-4c43-99de-0bfe9199ff95")]
        [TestCase("99ae8dd3-1067-4141-9675-62e94bb6caaa")]
        public void MethodGetByIdShould_CallRepositoryMethodGetById(string id)
        {
            var repositoryMock = new Mock<IRepository<User>>();
            var unitOfWorkMock = new Mock<IUnitOfWork>();
            var factoryMock = new Mock<IUsersFactory>();

            var service = new UsersService(repositoryMock.Object, unitOfWorkMock.Object, factoryMock.Object);
            service.GetById(id);

            repositoryMock.Verify(r => r.GetById(id), Times.Once);
        }

        [TestCase("d547a40d-c45f-4c43-99de-0bfe9199ff95")]
        [TestCase("99ae8dd3-1067-4141-9675-62e94bb6caaa")]
        public void MethodGetByIdShould_ReturnCorrectUser(string id)
        {
            var repositoryMock = new Mock<IRepository<User>>();
            var unitOfWorkMock = new Mock<IUnitOfWork>();
            var factoryMock = new Mock<IUsersFactory>();
            var foundUserMock = new Mock<User>();

            repositoryMock.Setup(r => r.GetById(id)).Returns(foundUserMock.Object);

            var service = new UsersService(repositoryMock.Object, unitOfWorkMock.Object, factoryMock.Object);
            var result = service.GetById(id);

            Assert.AreSame(foundUserMock.Object, result);
        }

        [TestCase("user1")]
        [TestCase("user2")]
        public void MethodGetByUsernameShould_CallRepositoryPropertyAll(string username)
        {
            var repositoryMock = new Mock<IRepository<User>>();
            var unitOfWorkMock = new Mock<IUnitOfWork>();
            var factoryMock = new Mock<IUsersFactory>();

            var service = new UsersService(repositoryMock.Object, unitOfWorkMock.Object, factoryMock.Object);
            service.GetByUsername(username);

            repositoryMock.Verify(r => r.All, Times.Once);
        }

        [TestCase("user1")]
        [TestCase("user2")]
        public void MethodGetByUsernameShould_ReturnCorrectUser(string username)
        {
            var repositoryMock = new Mock<IRepository<User>>();
            var unitOfWorkMock = new Mock<IUnitOfWork>();
            var factoryMock = new Mock<IUsersFactory>();
            var foundUser = new User() { UserName = username };

            repositoryMock.Setup(r => r.All).Returns(new List<User> { foundUser }.AsQueryable());

            var service = new UsersService(repositoryMock.Object, unitOfWorkMock.Object, factoryMock.Object);
            var result = service.GetByUsername(username);

            Assert.Contains(foundUser, result.ToList());
        }

        [TestCase("d547a40d-c45f-4c43-99de-0bfe9199ff95")]
        [TestCase("99ae8dd3-1067-4141-9675-62e94bb6caaa")]
        public void MethodGetUserWithBookingsShould_CallRepositoryPropertyAll(string id)
        {
            var repositoryMock = new Mock<IRepository<User>>();
            var unitOfWorkMock = new Mock<IUnitOfWork>();
            var factoryMock = new Mock<IUsersFactory>();

            var service = new UsersService(repositoryMock.Object, unitOfWorkMock.Object, factoryMock.Object);
            service.GetUserWithBookings(id);

            repositoryMock.Verify(r => r.All, Times.Once);
        }

        [TestCase("d547a40d-c45f-4c43-99de-0bfe9199ff95")]
        [TestCase("99ae8dd3-1067-4141-9675-62e94bb6caaa")]
        public void MethodGetUserWithBookingsShould_ReturnCorrectUserWithBookings(string id)
        {
            var repositoryMock = new Mock<IRepository<User>>();
            var unitOfWorkMock = new Mock<IUnitOfWork>();
            var factoryMock = new Mock<IUsersFactory>();
            var booking = new Booking() { Id = new Guid("d547a40d-c45f-4c43-99de-0bfe9199ff95") };
            var user = new User() { Id = id, Bookings = new List<Booking>() { booking } };

            repositoryMock.Setup(r => r.All).Returns(new List<User> { user }.AsQueryable<User>);
            var service = new UsersService(repositoryMock.Object, unitOfWorkMock.Object, factoryMock.Object);
            var result = service.GetUserWithBookings(id);

            Assert.AreSame(user, result);
        }
    }
}