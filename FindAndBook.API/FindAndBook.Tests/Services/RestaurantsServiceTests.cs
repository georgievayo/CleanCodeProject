

using FindAndBook.Data.Contracts;
using FindAndBook.Factories;
using FindAndBook.Models;
using FindAndBook.Services;
using Moq;
using NUnit.Framework;
using System;

namespace FindAndBook.Tests.Services
{
    [TestFixture]
    public class RestaurantsServiceTests
    {
        [TestCase("Rest", "0877142574", "09:00 - 12:00", "09:00 - 12:00", "some photo",
            "some details", 12, "d547a40d - c45f - 4c43 - 99de - 0bfe9199ff95",
            "Sofia")]
        public void CreaterestaurantShould_CallFactoryMethodCreaterestaurant(string name, string contact,
            string weekendHours, string weekdaayHours, string photo, string details, int? averageBill,
            string userId, string address)
        {
            var repositoryMock = new Mock<IRepository<Restaurant>>();
            var unitOfWorkMock = new Mock<IUnitOfWork>();
            var factoryMock = new Mock<IRestaurantsFactory>();

            var service = new RestaurantsService(repositoryMock.Object,
                unitOfWorkMock.Object, factoryMock.Object);

            service.Create(name, contact, weekendHours, weekdaayHours, photo,
                details, averageBill, userId, address);

            factoryMock.Verify(f => f.Create(name, contact, weekendHours, weekdaayHours, photo,
                details, averageBill, userId, address), Times.Once);
        }

        [TestCase("Rest", "0877142574", "09:00 - 12:00", "09:00 - 12:00", "some photo",
            "some details", 12, "d547a40d - c45f - 4c43 - 99de - 0bfe9199ff95",
            "Sofia")]
        public void CreaterestaurantShould_CallRepositoryMethodAdd(string name, string contact,
            string weekendHours, string weekdaayHours, string photo, string details, int? averageBill,
            string userId, string address)
        {
            var repositoryMock = new Mock<IRepository<Restaurant>>();
            var unitOfWorkMock = new Mock<IUnitOfWork>();
            var factoryMock = new Mock<IRestaurantsFactory>();

            var service = new RestaurantsService(repositoryMock.Object,
                unitOfWorkMock.Object, factoryMock.Object);

            var restaurant = new Restaurant()
            {
                ManagerId = userId,
                Address = address,
                Name = name,
                Contact = contact,
                WeekendHours = weekendHours,
                WeekdayHours = weekdaayHours,
                Details = details,
                AverageBill = averageBill,
            };
            factoryMock.Setup(f => f.Create(name, contact, weekendHours, weekdaayHours, photo,
                    details, averageBill, userId, address))
                .Returns(restaurant);

            service.Create(name, contact, weekendHours, weekdaayHours, photo,
                details, averageBill, userId, address);

            repositoryMock.Verify(f => f.Add(restaurant), Times.Once);
        }

        [TestCase("Rest", "0877142574", "09:00 - 12:00", "09:00 - 12:00", "some photo",
            "some details", 12, "d547a40d - c45f - 4c43 - 99de - 0bfe9199ff95",
            "Sofia")]
        public void CreaterestaurantShould_CallUnitOfWorkMethodACommit(string name, string contact,
            string weekendHours, string weekdaayHours, string photo, string details, int? averageBill,
            string userId, string address)
        {
            var repositoryMock = new Mock<IRepository<Restaurant>>();
            var unitOfWorkMock = new Mock<IUnitOfWork>();
            var factoryMock = new Mock<IRestaurantsFactory>();

            var service = new RestaurantsService(repositoryMock.Object,
                unitOfWorkMock.Object, factoryMock.Object);

            var restaurant = new Restaurant()
            {
                ManagerId = userId,
                Address = address,
                Name = name,
                Contact = contact,
                WeekendHours = weekendHours,
                WeekdayHours = weekdaayHours,
                Details = details,
                AverageBill = averageBill,
            };
            factoryMock.Setup(f => f.Create(name, contact, weekendHours, weekdaayHours, photo,
                    details, averageBill, userId, address))
                .Returns(restaurant);

            service.Create(name, contact, weekendHours, weekdaayHours, photo,
                details, averageBill, userId, address);

            unitOfWorkMock.Verify(f => f.Commit(), Times.Once);
        }

        [TestCase("Rest", "0877142574", "09:00 - 12:00", "09:00 - 12:00", "some photo",
            "some details", 12, "d547a40d - c45f - 4c43 - 99de - 0bfe9199ff95",
            "Sofia")]
        public void CreaterestaurantShould_ReturnCorrectResult(string name, string contact, string photo,
            string weekendHours, string weekdaayHours, string details, int? averageBill,
            string userId, string address)
        {
            var repositoryMock = new Mock<IRepository<Restaurant>>();
            var unitOfWorkMock = new Mock<IUnitOfWork>();
            var factoryMock = new Mock<IRestaurantsFactory>();

            var service = new RestaurantsService(repositoryMock.Object,
                unitOfWorkMock.Object, factoryMock.Object);

            var restaurant = new Restaurant()
            {
                ManagerId = userId,
                Address = address,
                Name = name,
                Contact = contact,
                WeekendHours = weekendHours,
                WeekdayHours = weekdaayHours,
                Details = details,
                AverageBill = averageBill,
            };
            factoryMock.Setup(f => f.Create(name, contact, weekendHours, weekdaayHours, photo,
                    details, averageBill, userId, address))
                .Returns(restaurant);

            var result = service.Create(name, contact, weekendHours, weekdaayHours, photo,
                details, averageBill, userId, address);

            Assert.AreSame(restaurant, result);
        }
    }
}
