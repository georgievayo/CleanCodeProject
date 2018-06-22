using FindAndBook.API.Controllers;
using FindAndBook.API.Mapper;
using FindAndBook.API.Models;
using FindAndBook.Models;
using FindAndBook.Providers.Contracts;
using FindAndBook.Services.Contracts;
using Moq;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
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

        [Test]
        public void ActionGetRestaurantDetailsShould_ReturnBadRequest_WhenPassedIdIsNull()
        {
            var result = controller.GetRestaurantDetails(null);

            Assert.IsInstanceOf<BadRequestResult>(result);
        }

        [Test]
        public void ActionGetRestaurantDetailsShould_ReturnBadRequest_WhenPassedIdIsEmpty()
        {
            var result = controller.GetRestaurantDetails("");

            Assert.IsInstanceOf<BadRequestResult>(result);
        }

        [Test]
        public void ActionGetRestaurantDetailsShould_ReturnBadRequest_WhenPassedIdIsNotValid()
        {
            var id = "not-valid";
            var result = controller.GetRestaurantDetails(id);

            Assert.IsInstanceOf<BadRequestErrorMessageResult>(result);
        }

        [Test]
        public void ActionGetRestaurantDetailsShould_CallServiceMethodGetById_WhenPassedIdIsCorrect()
        {
            var id = restaurant.Id.ToString();
            var result = controller.GetRestaurantDetails(id);

            restaurantsServiceMock.Verify(s => s.GetById(restaurant.Id), Times.Once);
        }

        [Test]
        public void ActionGetRestaurantDetailsShould_ReturnNotFound_WhenRestaurantWasNotFound()
        {
            var id = restaurant.Id.ToString();
            restaurantsServiceMock.Setup(s => s.GetById(restaurant.Id))
                .Returns(() => null);

            var result = controller.GetRestaurantDetails(id);

            Assert.IsInstanceOf<NotFoundResult>(result);
        }

        [Test]
        public void ActionGetRestaurantDetailsShould_CallMapper_WhenRestaurantWasFound()
        {
            var id = restaurant.Id.ToString();
            var result = controller.GetRestaurantDetails(id);

            mapperMock.Verify(m => m.MapRestaurant(restaurant), Times.Once);
        }

        [Test]
        public void ActionGetRestaurantDetailsShould_ReturnOk_WhenRestaurantWasFound()
        {
            var id = restaurant.Id.ToString();
            var mappedRestaurant = new RestaurantModel()
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

            mapperMock.Setup(m => m.MapRestaurant(restaurant))
                .Returns(mappedRestaurant);
            var result = controller.GetRestaurantDetails(id);

            Assert.IsInstanceOf<OkNegotiatedContentResult<RestaurantModel>>(result);
        }


        [Test]
        public void ActionSearchShould_CallServiceMethodGetAll_WhenSearchCriteriaIsNotPassed()
        {
            var criteria = new SearchCriteria();
            var result = controller.Search(criteria);

            restaurantsServiceMock.Verify(s => s.GetAll(), Times.Once);
        }


        [Test]
        public void ActionSearchShould_CallMapper_WhenSearchCriteriaIsNotPassed()
        {
            var criteria = new SearchCriteria();
            var foundRestaurants = new List<Restaurant>() { restaurant };
            var queryableRestaurants = foundRestaurants.AsQueryable();
            restaurantsServiceMock.Setup(s => s.GetAll())
                .Returns(() => queryableRestaurants);

            var result = controller.Search(criteria);

            mapperMock.Verify(m => m.MapRestaurantsCollection(queryableRestaurants), Times.Once);
        }

        [Test]
        public void ActionSearchShould_ReturnOkWithAllRestaurants_WhenSearchCriteriaIsNotPassed()
        {
            var criteria = new SearchCriteria();
            var foundRestaurants = new List<Restaurant>() { restaurant };
            var queryableRestaurants = foundRestaurants.AsQueryable();
            restaurantsServiceMock.Setup(s => s.GetAll())
                .Returns(() => queryableRestaurants);

            var result = controller.Search(criteria);

            Assert.IsInstanceOf<OkNegotiatedContentResult<List<RestaurantModel>>>(result);
        }

        [Test]
        public void ActionSearchShould_CallServiceMethodFindBy_WhenSearchCriteriaIsPassed()
        {
            var criteria = new SearchCriteria()
            {
                SearchBy = "name",
                Pattern = "t"
            };
           
            var result = controller.Search(criteria);

            restaurantsServiceMock.Verify(s => s.FindBy(criteria.SearchBy, criteria.Pattern), Times.Once);
        }

        [Test]
        public void ActionSearchShould_CallMapper_WhenSearchCriteriaIsPassed()
        {
            var criteria = new SearchCriteria()
            {
                SearchBy = "name",
                Pattern = "t"
            };
            var foundRestaurants = new List<Restaurant>() { restaurant };
            var queryableRestaurants = foundRestaurants.AsQueryable();
            restaurantsServiceMock.Setup(s => s.FindBy(It.IsAny<string>(), It.IsAny<string>()))
                .Returns(() => queryableRestaurants);

            var result = controller.Search(criteria);

            mapperMock.Verify(m => m.MapRestaurantsCollection(queryableRestaurants), Times.Once);
        }

        [Test]
        public void ActionSearchShould_ReturnOkAndFoundRestaurants_WhenSearchCriteriaIsPassed()
        {
            var criteria = new SearchCriteria()
            {
                SearchBy = "name",
                Pattern = "t"
            };
            var foundRestaurants = new List<Restaurant>() { restaurant };
            var queryableRestaurants = foundRestaurants.AsQueryable();
            restaurantsServiceMock.Setup(s => s.FindBy(It.IsAny<string>(), It.IsAny<string>()))
                .Returns(() => queryableRestaurants);

            var result = controller.Search(criteria);

            Assert.IsInstanceOf<OkNegotiatedContentResult<List<RestaurantModel>>>(result);
        }

        [Test]
        public void ActionGetManagerRestaurantsShould_ReturnBadRequest_WhenUserIdIsNull()
        {
            var result = controller.GetManagerRestaurants(null);

            Assert.IsInstanceOf<BadRequestResult>(result);
        }

        [Test]
        public void ActionGetManagerRestaurantsShould_ReturnBadRequest_WhenUserIdIsEmpty()
        {
            var result = controller.GetManagerRestaurants("");

            Assert.IsInstanceOf<BadRequestResult>(result);
        }

        [Test]
        public void ActionGetManagerRestaurantsShould_ReturnBadRequest_WhenUserIdIsNotValid()
        {
            var userId = "not-valid";
            var result = controller.GetManagerRestaurants(userId);

            Assert.IsInstanceOf<BadRequestErrorMessageResult>(result);
        }

        [Test]
        public void ActionGetManagerRestaurantsShould_GetCurrentUSerId_WhenUserIdIsValid()
        {
            var result = controller.GetManagerRestaurants(currentUserId.ToString());

            authProviderMock.Verify(ap => ap.CurrentUserID, Times.Once);
        }

        [Test]
        public void ActionGetManagerRestaurantsShould_ReturnConflict_WhenCurrentUserIdIsNotEqualToPassedUserId()
        {
            authProviderMock.Setup(ap => ap.CurrentUserID)
                .Returns(() => Guid.NewGuid());

            var result = controller.GetManagerRestaurants(currentUserId.ToString());

            Assert.IsInstanceOf<NegotiatedContentResult<string>>(result);
        }

        [Test]
        public void ActionGetManagerRestaurantsShould_CallServiceMethodGetRestaurantsOfManger_WhenCurrentUserIdIsEqualToPassedUserId()
        {
            var result = controller.GetManagerRestaurants(currentUserId.ToString());

            restaurantsServiceMock.Verify(s => s.GetRestaurantsOfManger(currentUserId), Times.Once);
        }

        [Test]
        public void ActionGetManagerRestaurantsShould_CallMapper_WhenCurrentUserIdIsEqualToPassedUserId()
        {
            var foundRestaurants = new List<Restaurant>() { restaurant };
            var queryableRestaurants = foundRestaurants.AsQueryable();
            restaurantsServiceMock.Setup(s => s.GetRestaurantsOfManger(It.IsAny<Guid>()))
                .Returns(() => queryableRestaurants);

            var result = controller.GetManagerRestaurants(currentUserId.ToString());

            mapperMock.Verify(m => m.MapRestaurantsCollection(queryableRestaurants), Times.Once);
        }

        [Test]
        public void ActionGetManagerRestaurantsShould_ReturnOkWithManagerRestaurants_WhenCurrentUserIdIsEqualToPassedUserId()
        {
            var foundRestaurants = new List<Restaurant>() { restaurant };
            var queryableRestaurants = foundRestaurants.AsQueryable();
            restaurantsServiceMock.Setup(s => s.GetRestaurantsOfManger(It.IsAny<Guid>()))
                .Returns(() => queryableRestaurants);

            var result = controller.GetManagerRestaurants(currentUserId.ToString());

            Assert.IsInstanceOf<OkNegotiatedContentResult<List<RestaurantModel>>>(result);
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
            restaurantsServiceMock.Setup(s => s.GetById(restaurant.Id))
                .Returns(() => restaurant);
            controller = new RestaurantsController(authProviderMock.Object, restaurantsServiceMock.Object, mapperMock.Object);
        }
    }
}
