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
using System.Text;
using System.Threading.Tasks;
using System.Web.Http.Results;

namespace FindAndBook.Tests.API
{
    [TestFixture]
    public class BookingsControllerTests
    {
        private Mock<IBookingsService> bookingsServiceMock;
        private Mock<IAuthenticationProvider> authProviderMock;
        private Mock<IModelsMapper> mapperMock;
        private BookingsController controller;
        private Guid currentUserId = Guid.NewGuid();
        private Booking booking;
        private IQueryable<Booking> queryableBookings;

        [Test]
        public void ActionBookATableShould_ReturnBadRequest_WhenRestaurantIdIsNull()
        {
            var result = controller.BookATable(null, null);

            Assert.IsInstanceOf<BadRequestErrorMessageResult>(result);
        }

        [Test]
        public void ActionBookATableShould_ReturnBadRequest_WhenModelIsNull()
        {
            var restaurantId = Guid.NewGuid();

            var result = controller.BookATable(restaurantId, null);

            Assert.IsInstanceOf<InvalidModelStateResult>(result);
        }

        [Test]
        public void ActionBookATableShould_ReturnBadRequest_WhenModelIsNotValid()
        {
            var restaurantId = Guid.NewGuid();
            controller.ModelState.AddModelError("Time", "Time is required");
            var result = controller.BookATable(restaurantId, null);

            Assert.IsInstanceOf<InvalidModelStateResult>(result);
        }

        [Test]
        public void ActionBookATableShould_GetCurrentUserId()
        {
            var id = Guid.NewGuid();
            var model = new BookingModel();

            var result = controller.BookATable(id, model);

            authProviderMock.Verify(ap => ap.CurrentUserID, Times.Once);
        }

        [Test]
        public void ActionBookATableShould_CallServiceMethodBookTable()
        {
            var id = Guid.NewGuid();
            var model = new BookingModel()
            {
                Time = new DateTime(),
                PeopleCount = 5
            };

            var result = controller.BookATable(id, model);

            bookingsServiceMock.Verify(s => s.BookTable(id, currentUserId, model.Time, model.PeopleCount), Times.Once);
        }

        [Test]
        public void ActionBookATableShould_ReturnConflict_WhenBookingWasNotCreated()
        {
            var id = Guid.NewGuid();
            var model = new BookingModel()
            {
                Time = new DateTime(),
                PeopleCount = 5
            };

            bookingsServiceMock.Setup(s => s.BookTable(id, currentUserId, model.Time, model.PeopleCount))
                .Returns(() => null);

            var result = controller.BookATable(id, model);

            Assert.IsInstanceOf<NegotiatedContentResult<string>>(result);
        }

        [Test]
        public void ActionBookATableShould_CallMapper_WhenBookingWasCreated()
        {
            var id = Guid.NewGuid();
            var model = new BookingModel()
            {
                Time = new DateTime(),
                PeopleCount = 5
            };

            bookingsServiceMock.Setup(s => s.BookTable(id, currentUserId, model.Time, model.PeopleCount))
                .Returns(() => booking);

            var result = controller.BookATable(id, model);

            mapperMock.Verify(m => m.MapBooking(booking));
        }

        [Test]
        public void ActionBookATableShould_ReturnOkWithBooking_WhenBookingWasCreated()
        {
            var id = Guid.NewGuid();
            var model = new BookingModel()
            {
                Time = new DateTime(),
                PeopleCount = 5
            };

            bookingsServiceMock.Setup(s => s.BookTable(id, currentUserId, model.Time, model.PeopleCount))
                .Returns(() => booking);

            var result = controller.BookATable(id, model);

            Assert.IsInstanceOf<OkNegotiatedContentResult<BookingModel>>(result);
        }

        [Test]
        public void ActionCancelBookingShould_ReturnBadRequest_WhenBookingIdIsNull()
        {
            var result = controller.CancelBooking(null);

            Assert.IsInstanceOf<BadRequestErrorMessageResult>(result);
        }

        [Test]
        public void ActionCancelBookingShould_CallServiceMethodGetById_WhenPassedIdIsValid()
        {
            var id = Guid.NewGuid();

            var result = controller.CancelBooking(id);

            bookingsServiceMock.Verify(s => s.GetById(id), Times.Once);
        }

        [Test]
        public void ActionCancelBookingShould_ReturnNotFound_WhenBookingWasNotFound()
        {
            var id = Guid.NewGuid();
            bookingsServiceMock.Setup(s => s.GetById(id))
                .Returns(() => null);

            var result = controller.CancelBooking(id);

            Assert.IsInstanceOf<NotFoundResult>(result);
        }

        [Test]
        public void ActionCancelBookingShould_GetCurrentUserId_WhenBookingWasFound()
        {
            var id = Guid.NewGuid();

            var result = controller.CancelBooking(id);

            authProviderMock.Verify(ap => ap.CurrentUserID, Times.Once);
        }

        [Test]
        public void ActionCancelBookingShould_ReturnForbidden_WhenCurrentUserIsNotOwnerOfBooking()
        {
            var id = Guid.NewGuid();
            authProviderMock.Setup(ap => ap.CurrentUserID)
                .Returns(() => Guid.NewGuid());

            var result = controller.CancelBooking(id);

            Assert.IsInstanceOf<NegotiatedContentResult<string>>(result);
        }

        [Test]
        public void ActionCancelBookingShould_CallServiceMethodDelete_WhenCurrentUserIsOwnerOfBooking()
        {
            var id = Guid.NewGuid();

            var result = controller.CancelBooking(id);

            bookingsServiceMock.Verify(s => s.Delete(booking), Times.Once);
        }

        [Test]
        public void ActionCancelBookingShould_Return_WhenBookingWasDeleted()
        {
            var id = Guid.NewGuid();

            var result = controller.CancelBooking(id);

            Assert.IsInstanceOf<OkResult>(result);
        }

        [Test]
        public void ActionGetAllShould_ReturnBadRequest_WhenBookingIdIsNull()
        {
            var result = controller.GetAll(null, null);

            Assert.IsInstanceOf<BadRequestErrorMessageResult>(result);
        }

        [Test]
        public void ActionGetAllShould_CallServiceMethodGetAllOfRestaurant_WhenTimeIsNotPassed()
        {
            var id = Guid.NewGuid();
            var criteria = new BookingCriteria();

            var result = controller.GetAll(id, criteria);

            bookingsServiceMock.Verify(s => s.GetAllOfRestaurant(id), Times.Once);
        }

        [Test]
        public void ActionGetAllShould_CallMapper_WhenTimeIsNotPassed()
        {
            var id = Guid.NewGuid();
            var criteria = new BookingCriteria();

            var result = controller.GetAll(id, criteria);

            mapperMock.Verify(m => m.MapBookingsCollection(queryableBookings), Times.Once);
        }

        [Test]
        public void ActionGetAllShould_ReturnOkWithBookings_WhenTimeIsNotPassed()
        {
            var id = Guid.NewGuid();
            var criteria = new BookingCriteria();

            var result = controller.GetAll(id, criteria);

            Assert.IsInstanceOf<OkNegotiatedContentResult<List<BookingModel>>>(result);
        }

        [Test]
        public void ActionGetAllShould_CallServiceMethodGetAllOn_WhenTimeIsPassed()
        {
            var id = Guid.NewGuid();
            var criteria = new BookingCriteria()
            {
                Time = new DateTime()
            };

            var result = controller.GetAll(id, criteria);

            bookingsServiceMock.Verify(s => s.GetAllOn((DateTime)criteria.Time, id), Times.Once);
        }

        [Test]
        public void ActionGetAllShould_CallMapper_WhenTimeIsPassed()
        {
            var id = Guid.NewGuid();
            var criteria = new BookingCriteria()
            {
                Time = new DateTime()
            };

            var result = controller.GetAll(id, criteria);

            mapperMock.Verify(m => m.MapBookingsCollection(queryableBookings), Times.Once);
        }

        [Test]
        public void ActionGetAllShould_ReturnOkWithBookings_WhenTimeIsPassed()
        {
            var id = Guid.NewGuid();
            var criteria = new BookingCriteria()
            {
                Time = new DateTime()
            };

            var result = controller.GetAll(id, criteria);

            Assert.IsInstanceOf<OkNegotiatedContentResult<List<BookingModel>>>(result);
        }

        [SetUp]
        public void SetUp()
        {
            bookingsServiceMock = new Mock<IBookingsService>();
            authProviderMock = new Mock<IAuthenticationProvider>();
            mapperMock = new Mock<IModelsMapper>();

            authProviderMock.Setup(ap => ap.CurrentUserRole)
                .Returns("Manager");
            authProviderMock.Setup(ap => ap.CurrentUserID)
                .Returns(currentUserId);

            booking = new Booking()
            {
                Id = Guid.NewGuid(),
                UserId = currentUserId,
                RestaurantId = Guid.NewGuid(),
                DateTime = new DateTime(),
                PeopleCount = 5
            };

            bookingsServiceMock.Setup(s => s.GetById(It.IsAny<Guid>()))
                .Returns(() => booking);
            var bookings = new List<Booking>() { booking };
            queryableBookings = bookings.AsQueryable();

            bookingsServiceMock.Setup(s => s.GetAllOfRestaurant(It.IsAny<Guid>()))
                .Returns(() => queryableBookings);
            bookingsServiceMock.Setup(s => s.GetAllOn(It.IsAny<DateTime>(), It.IsAny<Guid>()))
                .Returns(() => queryableBookings);

            controller = new BookingsController(bookingsServiceMock.Object, authProviderMock.Object, mapperMock.Object);
        }
    }
}
