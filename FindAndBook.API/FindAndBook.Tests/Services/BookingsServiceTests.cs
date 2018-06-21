using FindAndBook.Data.Contracts;
using FindAndBook.Factories;
using FindAndBook.Models;
using FindAndBook.Services;
using FindAndBook.Services.Contracts;
using Moq;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;

namespace FindAndBook.Tests.Services
{
    [TestFixture]
    public class BookingsServiceTests
    {
        private Mock<IRepository<Booking>> repositoryMock;
        private Mock<IUnitOfWork> unitOfWorkMock;
        private Mock<IBookingsFactory> factoryMock;
        private Mock<IRestaurantsService> restaurantsServiceMock;

        [TestCase(10)]
        public void MethodCreateShould_CallFactoryMethodCreateBooking(int peopleCount)
        {
            var service = new BookingsService(repositoryMock.Object,
                unitOfWorkMock.Object, factoryMock.Object, restaurantsServiceMock.Object);

            var restaurantId = Guid.NewGuid();
            var userId = Guid.NewGuid();
            var dateTime = DateTime.Now;

            service.Create(restaurantId, userId, dateTime, peopleCount);

            factoryMock.Verify(f => f.Create(restaurantId, userId, dateTime, peopleCount));
        }

        [TestCase(5)]
        public void MethodCreateShould_CallRepositoryMethodAdd(int peopleCount)
        {
            var service = new BookingsService(repositoryMock.Object,
                unitOfWorkMock.Object, factoryMock.Object, restaurantsServiceMock.Object);

            var restaurantId = Guid.NewGuid();
            var userId = Guid.NewGuid();

            var dateTime = DateTime.Now;
            var booking = new Booking()
            {
                RestaurantId = restaurantId,
                UserId = userId,
                DateTime = dateTime,
                PeopleCount = peopleCount
            };

            factoryMock.Setup(f => f.Create(restaurantId, userId, dateTime, peopleCount))
                .Returns(booking);

            service.Create(restaurantId, userId, dateTime, peopleCount);

            repositoryMock.Verify(r => r.Add(booking), Times.Once);
        }

        [Test]
        public void MethodGetAllOnShould_CallRepositoryMethodAll()
        {          
            var service = new BookingsService(repositoryMock.Object,
                unitOfWorkMock.Object, factoryMock.Object, restaurantsServiceMock.Object);
            var restaurantId = Guid.NewGuid();
            var dateTime = DateTime.Now;

            service.GetAllOn(dateTime, restaurantId);

            repositoryMock.Verify(r => r.All, Times.Once);
        }

        [Test]
        public void MethodGetAllOnShould_ReturnCorrectResult()
        {
            var restaurantId = Guid.NewGuid();
            var dateTime = DateTime.Now;
            var booking = new Booking() { RestaurantId = restaurantId, DateTime = dateTime };
            var list = new List<Booking>() { booking };
            repositoryMock.Setup(r => r.All).Returns(list.AsQueryable());

            var service = new BookingsService(repositoryMock.Object,
                unitOfWorkMock.Object, factoryMock.Object, restaurantsServiceMock.Object);

            var result = service.GetAllOn(dateTime, restaurantId);

            Assert.AreSame(booking, result.ToList().First());
        }

        [Test]
        public void MethodGetAllOfRestaurantShould_CallRepositoryMethodAll()
        {          
            var service = new BookingsService(repositoryMock.Object,
                unitOfWorkMock.Object, factoryMock.Object, restaurantsServiceMock.Object);
            var restaurantId = Guid.NewGuid();

            service.GetAllOfRestaurant(restaurantId);

            repositoryMock.Verify(r => r.All, Times.Once);
        }

        [Test]
        public void MethodGetAllOfRestaurantShould_ReturnCorrectResult()
        {
            var restaurantId = Guid.NewGuid();
            var booking = new Booking() { RestaurantId = restaurantId };
            var list = new List<Booking>() { booking };
            repositoryMock.Setup(r => r.All).Returns(list.AsQueryable());

            var service = new BookingsService(repositoryMock.Object,
                unitOfWorkMock.Object, factoryMock.Object, restaurantsServiceMock.Object);

            var result = service.GetAllOfRestaurant(restaurantId);

            Assert.AreSame(booking, result.ToList().First());
        }

        [Test]
        public void MethodGetByIdShould_CallRepositoryMethodGetById()
        {
            var id = Guid.NewGuid();
            var service = new BookingsService(repositoryMock.Object,
                unitOfWorkMock.Object, factoryMock.Object, restaurantsServiceMock.Object);

            service.GetById(id);

            repositoryMock.Verify(r => r.GetById(id));
        }

        [Test]
        public void MethodGetByIdShould_ReturnCorrectValue()
        {
            var id = Guid.NewGuid();
            var booking = new Booking() { Id = id };
            repositoryMock.Setup(r => r.GetById(id)).Returns(booking);

            var service = new BookingsService(repositoryMock.Object,
                unitOfWorkMock.Object, factoryMock.Object, restaurantsServiceMock.Object);

            var result = service.GetById(id);

            Assert.AreSame(booking, result);
        }

        [Test]
        public void MethodDeleteShould_CallRepositoryMethodDelete()
        {
            var booking = new Booking() { Id = Guid.NewGuid() };

            var service = new BookingsService(repositoryMock.Object,
                unitOfWorkMock.Object, factoryMock.Object, restaurantsServiceMock.Object);

            service.Delete(booking);

            repositoryMock.Verify(r => r.Delete(booking), Times.Once);
        }

        [Test]
        public void MethodDeleteShould_CallUnitOfWorkMethodCommit()
        {
            var booking = new Booking() { Id = Guid.NewGuid() };

            var service = new BookingsService(repositoryMock.Object,
                unitOfWorkMock.Object, factoryMock.Object, restaurantsServiceMock.Object);

            service.Delete(booking);

            unitOfWorkMock.Verify(r => r.Commit(), Times.Once);
        }

        [SetUp]
        public void SetUp()
        {
            repositoryMock = new Mock<IRepository<Booking>>();
            unitOfWorkMock = new Mock<IUnitOfWork>();
            factoryMock = new Mock<IBookingsFactory>();
            restaurantsServiceMock = new Mock<IRestaurantsService>();
        }
    }
}
