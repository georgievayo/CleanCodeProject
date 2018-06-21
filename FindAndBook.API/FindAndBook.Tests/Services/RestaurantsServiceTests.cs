

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
            "some details", 12, "Sofia", 58)]
        public void MethodCreateShould_CallFactoryMethodCreaterestaurant(string name, string contact,
            string weekendHours, string weekdaayHours, string photo, string details, int averageBill,
            string address, int maxPeopleCount)
        {
            var repositoryMock = new Mock<IRepository<Restaurant>>();
            var unitOfWorkMock = new Mock<IUnitOfWork>();
            var factoryMock = new Mock<IRestaurantsFactory>();

            var managerId = Guid.NewGuid();

            var service = new RestaurantsService(repositoryMock.Object,
                unitOfWorkMock.Object, factoryMock.Object);

            service.Create(name, contact, weekendHours, weekdaayHours, photo,
                details, averageBill, managerId, address, maxPeopleCount);

            factoryMock.Verify(f => f.Create(name, contact, weekendHours, weekdaayHours, photo,
                details, averageBill, managerId, address, maxPeopleCount), Times.Once);
        }

        [TestCase("Rest", "0877142574", "09:00 - 12:00", "09:00 - 12:00", "some photo",
            "some details", 12, "Sofia", 58)]
        public void MethodCreateShould_CallRepositoryMethodAdd(string name, string contact,
            string weekendHours, string weekdaayHours, string photo, string details, int averageBill,
            string address, int maxPeopleCount)
        {
            var repositoryMock = new Mock<IRepository<Restaurant>>();
            var unitOfWorkMock = new Mock<IUnitOfWork>();
            var factoryMock = new Mock<IRestaurantsFactory>();

            var managerId = Guid.NewGuid();

            var service = new RestaurantsService(repositoryMock.Object,
                unitOfWorkMock.Object, factoryMock.Object);

            var restaurant = new Restaurant()
            {
                ManagerId = managerId,
                Address = address,
                Name = name,
                Contact = contact,
                WeekendHours = weekendHours,
                WeekdayHours = weekdaayHours,
                Details = details,
                AverageBill = averageBill,
            };
            factoryMock.Setup(f => f.Create(name, contact, weekendHours, weekdaayHours, photo,
                    details, averageBill, managerId, address, maxPeopleCount))
                .Returns(restaurant);

            service.Create(name, contact, weekendHours, weekdaayHours, photo,
                details, averageBill, managerId, address, maxPeopleCount);

            repositoryMock.Verify(f => f.Add(restaurant), Times.Once);
        }

        [TestCase("Rest", "0877142574", "09:00 - 12:00", "09:00 - 12:00", "some photo",
            "some details", 12, "Sofia", 58)]
        public void MethodCreateShould_CallUnitOfWorkMethodCommit(string name, string contact,
            string weekendHours, string weekdaayHours, string photo, string details, int averageBill,
            string address, int maxPeopleCount)
        {
            var repositoryMock = new Mock<IRepository<Restaurant>>();
            var unitOfWorkMock = new Mock<IUnitOfWork>();
            var factoryMock = new Mock<IRestaurantsFactory>();

            var service = new RestaurantsService(repositoryMock.Object,
                unitOfWorkMock.Object, factoryMock.Object);

            var managerId = Guid.NewGuid();

            var restaurant = new Restaurant()
            {
                ManagerId = managerId,
                Address = address,
                Name = name,
                Contact = contact,
                WeekendHours = weekendHours,
                WeekdayHours = weekdaayHours,
                Details = details,
                AverageBill = averageBill,
                MaxPeopleCount = maxPeopleCount
            };
            factoryMock.Setup(f => f.Create(name, contact, weekendHours, weekdaayHours, photo,
                    details, averageBill, managerId, address,maxPeopleCount))
                .Returns(restaurant);

            service.Create(name, contact, weekendHours, weekdaayHours, photo,
                details, averageBill, managerId, address, maxPeopleCount);

            unitOfWorkMock.Verify(f => f.Commit(), Times.Once);
        }

        [TestCase("Rest", "0877142574", "09:00 - 12:00", "09:00 - 12:00", "some photo",
            "some details", 12, "Sofia", 58)]
        public void MethodCreateShould_ReturnCorrectResult(string name, string contact, string photo,
            string weekendHours, string weekdaayHours, string details, int averageBill,
            string address, int maxPeopleCount)
        {
            var repositoryMock = new Mock<IRepository<Restaurant>>();
            var unitOfWorkMock = new Mock<IUnitOfWork>();
            var factoryMock = new Mock<IRestaurantsFactory>();

            var managerId = Guid.NewGuid();

            var service = new RestaurantsService(repositoryMock.Object,
                unitOfWorkMock.Object, factoryMock.Object);

            var restaurant = new Restaurant()
            {
                ManagerId = managerId,
                Address = address,
                Name = name,
                Contact = contact,
                WeekendHours = weekendHours,
                WeekdayHours = weekdaayHours,
                Details = details,
                AverageBill = averageBill,
            };

            factoryMock.Setup(f => f.Create(name, contact, weekendHours, weekdaayHours, photo,
                    details, averageBill, managerId, address, maxPeopleCount))
                .Returns(restaurant);

            var result = service.Create(name, contact, weekendHours, weekdaayHours, photo,
                details, averageBill, managerId, address, maxPeopleCount);

            Assert.AreSame(restaurant, result);
        }

        [TestCase("0877142574", "some details", "url", "09:00 - 12:00", "09:00 - 12:00", 12, 58)]
        public void MethodEditShould_CallRepositoryMethodGetById(string contact, string details,
            string photoUrl, string weekendHours, string weekdaayHours, int averageBill, int maxPeopleCount)
        {
            var repositoryMock = new Mock<IRepository<Restaurant>>();
            var unitOfWorkMock = new Mock<IUnitOfWork>();
            var factoryMock = new Mock<IRestaurantsFactory>();

            var service = new RestaurantsService(repositoryMock.Object,
                unitOfWorkMock.Object, factoryMock.Object);
            var id = Guid.NewGuid();
            service.Edit(id, contact, details, photoUrl, weekdaayHours, weekendHours, averageBill, maxPeopleCount);

            repositoryMock.Verify(r => r.GetById(id), Times.Once);
        }

        [TestCase("0877142574", "some details", "url", "09:00 - 12:00", "09:00 - 12:00", 12, 58)]
        public void EditPlaceShould_ReturnNull_WhenPlaceWasNotFound(string contact, string details,
            string photoUrl, string weekendHours, string weekdaayHours, int averageBill, int maxPeopleCount)
        {
            var repositoryMock = new Mock<IRepository<Restaurant>>();
            var unitOfWorkMock = new Mock<IUnitOfWork>();
            var factoryMock = new Mock<IRestaurantsFactory>();

            var service = new RestaurantsService(repositoryMock.Object,
                unitOfWorkMock.Object, factoryMock.Object);
            var id = Guid.NewGuid();
            repositoryMock.Setup(r => r.GetById(id)).Returns((Restaurant)null);

            var result = service.Edit(id, contact, details, photoUrl, weekdaayHours, weekendHours, averageBill, maxPeopleCount);

            Assert.IsNull(result);
        }

        [TestCase("0877142574", "some details", "url", "09:00 - 12:00", "09:00 - 12:00", 12, 58)]
        public void EditPlaceShould_CallRepositoryMethodUpdate(string contact, string details,
            string photoUrl, string weekendHours, string weekdaayHours, int averageBill, int maxPeopleCount)
        {
            var repositoryMock = new Mock<IRepository<Restaurant>>();
            var unitOfWorkMock = new Mock<IUnitOfWork>();
            var factoryMock = new Mock<IRestaurantsFactory>();

            var service = new RestaurantsService(repositoryMock.Object,
                unitOfWorkMock.Object, factoryMock.Object);
            var id = Guid.NewGuid();
            var place = new Restaurant()
            {
                Id = id,
                Contact = "contact",
                WeekendHours = "00:00 - 00:00",
                WeekdayHours = "00:00 - 00:00",
                Details = "details",
                AverageBill = 0,
            };
            repositoryMock.Setup(r => r.GetById(id)).Returns(place);

            var edittedPlace = place;
            edittedPlace.Contact = contact;
            edittedPlace.WeekendHours = weekendHours;
            edittedPlace.WeekdayHours = weekdaayHours;
            edittedPlace.Details = details;
            edittedPlace.AverageBill = averageBill;
            edittedPlace.MaxPeopleCount = maxPeopleCount;


            service.Edit(id, contact, details, photoUrl, weekdaayHours, weekendHours, averageBill, maxPeopleCount);

            repositoryMock.Verify(r => r.Update(edittedPlace), Times.Once);
        }

        [TestCase("0877142574", "some details", "url", "09:00 - 12:00", "09:00 - 12:00", 12, 58)]
        public void EditPlaceShould_CallUnitOfWorkMethodCommit(string contact, string details,
            string photoUrl, string weekendHours, string weekdaayHours, int averageBill, int maxPeopleCount)
        {
            var repositoryMock = new Mock<IRepository<Restaurant>>();
            var unitOfWorkMock = new Mock<IUnitOfWork>();
            var factoryMock = new Mock<IRestaurantsFactory>();

            var service = new RestaurantsService(repositoryMock.Object,
                unitOfWorkMock.Object, factoryMock.Object);
            var id = Guid.NewGuid();
            var place = new Restaurant()
            {
                Id = id,
                Contact = "contact",
                WeekendHours = "00:00 - 00:00",
                WeekdayHours = "00:00 - 00:00",
                Details = "details",
                AverageBill = 0,
            };
            repositoryMock.Setup(r => r.GetById(id)).Returns(place);

            service.Edit(id, contact, details, photoUrl, weekdaayHours, weekendHours, averageBill, maxPeopleCount);

            unitOfWorkMock.Verify(u => u.Commit(), Times.Once);
        }

        [TestCase("0877142574", "some details", "url", "09:00 - 12:00", "09:00 - 12:00", 12, 58)]
        public void EditPlaceShould_ReturnCorrectResult(string contact, string details,
            string photoUrl, string weekendHours, string weekdaayHours, int averageBill, int maxPeopleCount)
        {
            var repositoryMock = new Mock<IRepository<Restaurant>>();
            var unitOfWorkMock = new Mock<IUnitOfWork>();
            var factoryMock = new Mock<IRestaurantsFactory>();

            var service = new RestaurantsService(repositoryMock.Object,
                unitOfWorkMock.Object, factoryMock.Object);
            var id = Guid.NewGuid();
            var place = new Restaurant()
            {
                Id = id,
                Contact = "contact",
                WeekendHours = "00:00 - 00:00",
                WeekdayHours = "00:00 - 00:00",
                Details = "details",
                AverageBill = 0,
            };
            repositoryMock.Setup(r => r.GetById(id)).Returns(place);

            var edittedPlace = new Restaurant()
            {
                Id = id,
                Contact = contact,
                WeekendHours = weekendHours,
                WeekdayHours = weekdaayHours,
                Details = details,
                AverageBill = averageBill,
                MaxPeopleCount = maxPeopleCount
            };

            var result = service.Edit(id, contact, details, photoUrl, weekdaayHours, weekendHours, averageBill, maxPeopleCount);

            Assert.AreEqual(contact, result.Contact);
            Assert.AreEqual(details, result.Details);
            Assert.AreEqual(photoUrl, result.PhotoUrl);
            Assert.AreEqual(weekdaayHours, result.WeekdayHours);
            Assert.AreEqual(weekendHours, result.WeekendHours);
            Assert.AreEqual(averageBill, result.AverageBill);
        }
    }
}
