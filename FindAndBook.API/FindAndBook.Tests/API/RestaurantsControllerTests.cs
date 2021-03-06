﻿using FindAndBook.API.Controllers;
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

            Assert.IsInstanceOf<BadRequestErrorMessageResult>(result);
        }

        [Test]
        public void ActionGetRestaurantDetailsShould_CallServiceMethodGetById_WhenPassedIdIsCorrect()
        {
            var id = restaurant.Id;
            var result = controller.GetRestaurantDetails(id);

            restaurantsServiceMock.Verify(s => s.GetById(restaurant.Id), Times.Once);
        }

        [Test]
        public void ActionGetRestaurantDetailsShould_ReturnNotFound_WhenRestaurantWasNotFound()
        {
            var id = restaurant.Id;
            restaurantsServiceMock.Setup(s => s.GetById(id))
                .Returns(() => null);

            var result = controller.GetRestaurantDetails(id);

            Assert.IsInstanceOf<NotFoundResult>(result);
        }

        [Test]
        public void ActionGetRestaurantDetailsShould_CallMapper_WhenRestaurantWasFound()
        {
            var id = restaurant.Id;
            var result = controller.GetRestaurantDetails(id);

            mapperMock.Verify(m => m.MapRestaurant(restaurant), Times.Once);
        }

        [Test]
        public void ActionGetRestaurantDetailsShould_ReturnOk_WhenRestaurantWasFound()
        {
            var id = restaurant.Id;
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

            Assert.IsInstanceOf<BadRequestErrorMessageResult>(result);
        }

        [Test]
        public void ActionGetManagerRestaurantsShould_GetCurrentUSerId_WhenUserIdIsValid()
        {
            var result = controller.GetManagerRestaurants(currentUserId);

            authProviderMock.Verify(ap => ap.CurrentUserID, Times.Once);
        }

        [Test]
        public void ActionGetManagerRestaurantsShould_ReturnConflict_WhenCurrentUserIdIsNotEqualToPassedUserId()
        {
            authProviderMock.Setup(ap => ap.CurrentUserID)
                .Returns(() => Guid.NewGuid());

            var result = controller.GetManagerRestaurants(currentUserId);

            Assert.IsInstanceOf<NegotiatedContentResult<string>>(result);
        }

        [Test]
        public void ActionGetManagerRestaurantsShould_CallServiceMethodGetRestaurantsOfManger_WhenCurrentUserIdIsEqualToPassedUserId()
        {
            var result = controller.GetManagerRestaurants(currentUserId);

            restaurantsServiceMock.Verify(s => s.GetRestaurantsOfManger(currentUserId), Times.Once);
        }

        [Test]
        public void ActionGetManagerRestaurantsShould_CallMapper_WhenCurrentUserIdIsEqualToPassedUserId()
        {
            var foundRestaurants = new List<Restaurant>() { restaurant };
            var queryableRestaurants = foundRestaurants.AsQueryable();
            restaurantsServiceMock.Setup(s => s.GetRestaurantsOfManger(It.IsAny<Guid>()))
                .Returns(() => queryableRestaurants);

            var result = controller.GetManagerRestaurants(currentUserId);

            mapperMock.Verify(m => m.MapRestaurantsCollection(queryableRestaurants), Times.Once);
        }

        [Test]
        public void ActionGetManagerRestaurantsShould_ReturnOkWithManagerRestaurants_WhenCurrentUserIdIsEqualToPassedUserId()
        {
            var foundRestaurants = new List<Restaurant>() { restaurant };
            var queryableRestaurants = foundRestaurants.AsQueryable();
            restaurantsServiceMock.Setup(s => s.GetRestaurantsOfManger(It.IsAny<Guid>()))
                .Returns(() => queryableRestaurants);

            var result = controller.GetManagerRestaurants(currentUserId);

            Assert.IsInstanceOf<OkNegotiatedContentResult<List<RestaurantModel>>>(result);
        }

        [Test]
        public void ActionEditRestaurantShould_ReturnBadRequest_WhenRestaurantIdIsNull()
        {
            var result = controller.EditRestaurant(null, null);

            Assert.IsInstanceOf<BadRequestErrorMessageResult>(result);
        }

        [Test]
        public void ActionEditRestaurantShould_ReturnBadRequest_WhenModelIsNull()
        {
            var restaurantId = Guid.NewGuid();
            var result = controller.EditRestaurant(restaurantId, null);

            Assert.IsInstanceOf<InvalidModelStateResult>(result);
        }

        [Test]
        public void ActionEditRestaurantShould_ReturnBadRequest_WhenModelIsNotValid()
        {
            var restaurantId = Guid.NewGuid();

            controller.ModelState.AddModelError("Name", "Name is required.");
            var model = new RestaurantModel();

            var result = controller.EditRestaurant(restaurantId, model);

            Assert.IsInstanceOf<InvalidModelStateResult>(result);
        }

        [Test]
        public void ActionEditRestaurantShould_CallServiceMethodGetById_WhenPassedIdIsValid()
        {
            var id = Guid.NewGuid();
            var model = new RestaurantModel();

            var result = controller.EditRestaurant(id, model);

            restaurantsServiceMock.Verify(s => s.GetById(id), Times.Once);
        }

        [Test]
        public void ActionEditRestaurantShould_ReturnNotFound_WhenRestaurantWasNotFound()
        {
            var id = Guid.NewGuid();
            var model = new RestaurantModel();

            restaurantsServiceMock.Setup(s => s.GetById(id))
                .Returns(() => null);

            var result = controller.EditRestaurant(id, model);

            Assert.IsInstanceOf<NotFoundResult>(result);
        }

        [Test]
        public void ActionEditRestaurantShould_GetCurrentUserId_WhenRestaurantWasFound()
        {
            var id = Guid.NewGuid();
            var model = new RestaurantModel();

            var result = controller.EditRestaurant(id, model);

            authProviderMock.Verify(ap => ap.CurrentUserID, Times.Once);
        }

        [Test]
        public void ActionEditRestaurantShould_ReturnForbidden_WhenCurrentUserIsNotManagerOfRestaurant()
        {
            var id = Guid.NewGuid();
            var model = new RestaurantModel();

            authProviderMock.Setup(s => s.CurrentUserID)
                .Returns(() => Guid.NewGuid());

            var result = controller.EditRestaurant(id, model);

            Assert.IsInstanceOf<NegotiatedContentResult<string>>(result);
        }

        [Test]
        public void ActionEditRestaurantShould_CallServiceMethodEdit_WhenCurrentUserIsManagerOfRestaurant()
        {
            var id = Guid.NewGuid();
            var model = new RestaurantModel()
            {
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

            var result = controller.EditRestaurant(id, model);

            restaurantsServiceMock.Verify(s => s.Edit(id, model.Contact, model.Details, 
                model.PhotoUrl, model.WeekdayHours, model.WeekendHours, model.AverageBill, 
                model.MaxPeopleCount), Times.Once);
        }

        [Test]
        public void ActionEditRestaurantShould_CallMapper_WhenRestaurantIsUpdated()
        {
            var id = Guid.NewGuid();
            var model = new RestaurantModel()
            {
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

            var result = controller.EditRestaurant(id, model);

            mapperMock.Verify(m => m.MapRestaurant(restaurant));
        }

        [Test]
        public void ActionEditRestaurantShould_ReturnOkWithUpdatedRestaurant_WhenRestaurantIsUpdated()
        {
            var id = Guid.NewGuid();
            var model = new RestaurantModel()
            {
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

            var result = controller.EditRestaurant(id, model);

            Assert.IsInstanceOf<OkNegotiatedContentResult<RestaurantModel>>(result);
        }

        [Test]
        public void ActionDeleteRestaurantShould_ReturnBadRequest_WhenRestaurantIdIsNull()
        {
            var result = controller.DeleteRestaurant(null);

            Assert.IsInstanceOf<BadRequestErrorMessageResult>(result);
        }

        [Test]
        public void ActionDeleteRestaurantShould_CallServiceMethodGetById_WhenPassedIdIsValid()
        {
            var id = Guid.NewGuid();

            var result = controller.DeleteRestaurant(id);

            restaurantsServiceMock.Verify(s => s.GetById(id), Times.Once);
        }

        [Test]
        public void ActionDeleteRestaurantShould_ReturnNotFound_WhenRestaurantWasNotFound()
        {
            var id = Guid.NewGuid();

            restaurantsServiceMock.Setup(s => s.GetById(id))
                .Returns(() => null);

            var result = controller.DeleteRestaurant(id);

            Assert.IsInstanceOf<NotFoundResult>(result);
        }

        [Test]
        public void ActionDeleteRestaurantShould_GetCurrentUserId_WhenRestaurantWasFound()
        {
            var id = Guid.NewGuid();

            var result = controller.DeleteRestaurant(id);

            authProviderMock.Verify(ap => ap.CurrentUserID, Times.Once);
        }

        [Test]
        public void ActionDeleteRestaurantShould_ReturnForbidden_WhenCurrentUserIsNotManagerOfRestaurant()
        {
            var id = Guid.NewGuid();

            authProviderMock.Setup(s => s.CurrentUserID)
                .Returns(() => Guid.NewGuid());

            var result = controller.DeleteRestaurant(id);

            Assert.IsInstanceOf<NegotiatedContentResult<string>>(result);
        }

        [Test]
        public void ActionDeleteRestaurantShould_CallServiceMethodDelete_WhenCurrentUserIsManagerOfRestaurant()
        {
            var id = Guid.NewGuid();

            var result = controller.DeleteRestaurant(id);

            restaurantsServiceMock.Verify(s => s.Delete(id), Times.Once);
        }

        [Test]
        public void ActionDeleteRestaurantShould_ReturnOk_WhenRestaurantWasDeleted()
        {
            var id = Guid.NewGuid();

            var result = controller.DeleteRestaurant(id);

            Assert.IsInstanceOf<OkResult>(result);
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
            restaurantsServiceMock.Setup(s => s.Edit(It.IsAny<Guid>(), It.IsAny<string>(),
                It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>(),
                It.IsAny<int>(), It.IsAny<int>()))
                .Returns(() => restaurant);
            restaurantsServiceMock.Setup(s => s.GetById(It.IsAny<Guid>()))
                .Returns(() => restaurant);

            controller = new RestaurantsController(authProviderMock.Object, restaurantsServiceMock.Object, mapperMock.Object);
        }
    }
}
