using FindAndBook.API.Controllers;
using FindAndBook.API.Mapper;
using FindAndBook.API.Models;
using FindAndBook.Models;
using FindAndBook.Providers.Contracts;
using FindAndBook.Services.Contracts;
using Moq;
using NUnit.Framework;
using System;
using System.Web.Http.Results;

namespace FindAndBook.Tests.API
{
    [TestFixture]
    public class RestaurantsControllerTests
    {
        private Mock<IRestaurantsService> restaurantsServiceMock;
        private Mock<IAuthenticationProvider> authProviderMock;
        private Mock<IModelsMapper> mapperMock;
        private RestaurantsController controller;
        private Guid currentUserId = Guid.NewGuid();
        private Restaurant restaurant;

        [Test]
        public void ActionCreateRestaurantShould_ReturnBadRequest_WhenModelIsNull()
        {
            var result = controller.CreateRestaurant(null);

            Assert.IsInstanceOf<InvalidModelStateResult>(result);
        }

        [Test]
        public void ActionCreateRestaurantShould_ReturnBadRequest_WhenModelIsNotValid()
        {
            controller.ModelState.AddModelError("Name", "Name is required.");
            var model = new RestaurantModel();

            var result = controller.CreateRestaurant(model);

            Assert.IsInstanceOf<InvalidModelStateResult>(result);
        }

        [Test]
        public void ActionCreateRestaurantShould_ReturnForbidden_WhenCurrentUserIsNotManager()
        {
            var model = new RestaurantModel();
            authProviderMock.Setup(ap => ap.CurrentUserRole)
                .Returns("User");

            var result = controller.CreateRestaurant(model);

            Assert.IsInstanceOf<NegotiatedContentResult<string>>(result);
        }

        [Test]
        public void ActionCreateRestaurantShould_GetCurrentUserID_WhenCurrentUserIsManager()
        {
            var model = new RestaurantModel()
            {
                Id = Guid.NewGuid(),
                Name = "Restaurant",
                Address = "test",
                Contact = "test",
                WeekdayHours = "09:00 - 22:00",
                WeekendHours = "09:00 - 22:00",
                PhotoUrl = "test",
                MaxPeopleCount = 20,
                AverageBill = 5,
                Details = "test"
            };

            var result = controller.CreateRestaurant(model);

            authProviderMock.Verify(ap => ap.CurrentUserID, Times.Once);
        }

        [Test]
        public void ActionCreateRestaurantShould_CallServiceMethodCreate_WhenCurrentUserIsManager()
        {
            var model = new RestaurantModel()
            {
                Id = Guid.NewGuid(),
                Name = "Restaurant",
                Address = "test",
                Contact = "test",
                WeekdayHours = "09:00 - 22:00",
                WeekendHours = "09:00 - 22:00",
                PhotoUrl = "test",
                MaxPeopleCount = 20,
                AverageBill = 5,
                Details = "test"
            };

            var result = controller.CreateRestaurant(model);

            restaurantsServiceMock.Verify(s =>s.Create(model.Name, model.Contact, model.WeekendHours,
                model.WeekdayHours, model.PhotoUrl, model.Details, model.AverageBill, 
                currentUserId, model.Address, model.MaxPeopleCount), Times.Once);
        }

        [Test]
        public void ActionCreateRestaurantShould_CallMapper_WhenRestaurantWasCreated()
        {
            var model = new RestaurantModel()
            {
                Id = Guid.NewGuid(),
                Name = "Restaurant",
                Address = "test",
                Contact = "test",
                WeekdayHours = "09:00 - 22:00",
                WeekendHours = "09:00 - 22:00",
                PhotoUrl = "test",
                MaxPeopleCount = 20,
                AverageBill = 5,
                Details = "test"
            };

            var result = controller.CreateRestaurant(model);

            mapperMock.Verify(m => m.MapRestaurant(restaurant), Times.Once);
        }

        [Test]
        public void ActionCreateRestaurantShould_ReturnOk_WhenRestaurantWasCreated()
        {
            var model = new RestaurantModel()
            {
                Id = Guid.NewGuid(),
                Name = "Restaurant",
                Address = "test",
                Contact = "test",
                WeekdayHours = "09:00 - 22:00",
                WeekendHours = "09:00 - 22:00",
                PhotoUrl = "test",
                MaxPeopleCount = 20,
                AverageBill = 5,
                Details = "test"
            };

            var result = controller.CreateRestaurant(model);

            Assert.IsInstanceOf<OkNegotiatedContentResult<RestaurantModel>>(result);
        }

        [SetUp]
        public void SetUp()
        {
            restaurantsServiceMock = new Mock<IRestaurantsService>();
            authProviderMock = new Mock<IAuthenticationProvider>();
            mapperMock = new Mock<IModelsMapper>();

            authProviderMock.Setup(ap => ap.CurrentUserRole)
                .Returns("Manager");
            authProviderMock.Setup(ap => ap.CurrentUserID)
                .Returns(currentUserId);
            restaurant = new Restaurant()
            {
                Id = Guid.NewGuid(),
                Name = "Restaurant",
                Address = "test",
                Contact = "test",
                WeekdayHours = "09:00 - 22:00",
                WeekendHours = "09:00 - 22:00",
                PhotoUrl = "test",
                MaxPeopleCount = 20,
                ManagerId = currentUserId,
                AverageBill = 5,
                Details = "test"
            };

            restaurantsServiceMock.Setup(s => s.Create(It.IsAny<string>(), It.IsAny<string>(),
                It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>(),
                It.IsAny<int>(), It.IsAny<Guid>(), It.IsAny<string>(), It.IsAny<int>()))
                .Returns(() => restaurant);

            controller = new RestaurantsController(authProviderMock.Object, restaurantsServiceMock.Object, mapperMock.Object);
        }
    }
}
