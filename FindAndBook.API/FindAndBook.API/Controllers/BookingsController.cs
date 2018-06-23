using FindAndBook.API.App_Start;
using FindAndBook.API.Mapper;
using FindAndBook.API.Models;
using FindAndBook.Models;
using FindAndBook.Providers.Contracts;
using FindAndBook.Services.Contracts;
using System;
using System.Linq;
using System.Web.Http;

namespace FindAndBook.API.Controllers
{
    public class BookingsController : ApiController
    {
        private readonly IBookingsService bookingsService;

        private readonly IAuthenticationProvider authProvider;

        private readonly IModelsMapper mapper;

        public BookingsController(IBookingsService bookingsService, IAuthenticationProvider authProvider, IModelsMapper mapper)
        {
            this.bookingsService = bookingsService;
            this.authProvider = authProvider;
            this.mapper = mapper;
        }

        [HttpPost]
        [Route("api/restaurants/{id}/bookings")]
        public IHttpActionResult BookATable([FromUri] Guid? id, BookingModel model)
        {
            if (id == null)
            {
                return BadRequest(Constants.REQUIRED_RESTAURANT_ID);
            }

            if (model == null || !ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var userId = this.authProvider.CurrentUserID;

            var booking = this.bookingsService.BookTable((Guid)id, userId, model.Time, model.PeopleCount);
            if (booking == null)
            {
                return Content(System.Net.HttpStatusCode.Conflict, Constants.NO_AVAILABLE_TABLES);
            }
            else
            {
                var response = this.mapper.MapBooking(booking);

                return Ok(response);
            }
        }

        [HttpDelete]
        [Route("api/bookings/{id}")]
        public IHttpActionResult CancelBooking([FromUri] Guid? id)
        {
            if (id == null)
            {
                return BadRequest(Constants.REQUIRED_BOOKING_ID);
            }

            var booking = this.bookingsService.GetById((Guid)id);
            if (booking == null)
            {
                return NotFound();
            }
            else
            {
                var currentUserId = this.authProvider.CurrentUserID;
                if (booking.UserId != currentUserId)
                {
                    return Content(System.Net.HttpStatusCode.Forbidden, Constants.FORBIDDEN_CANCEL_BOOKING);
                }
                else
                {
                    this.bookingsService.Delete(booking);
                    return Ok();
                }
            }
        }

        [HttpGet]
        [Route("api/restaurants/{id}/bookings")]
        public IHttpActionResult GetAll([FromUri] Guid? id, [FromUri] BookingCriteria criteria)
        {
            if (id == null)
            {
                return BadRequest(Constants.REQUIRED_RESTAURANT_ID);
            }

            IQueryable<Booking> bookings = null;

            if (criteria.Time == null)
            {
                bookings = this.bookingsService.GetAllOfRestaurant((Guid)id);
            }
            else
            {
                var time = (DateTime)criteria.Time;
                bookings = this.bookingsService.GetAllOn(time, (Guid)id);
            }

            var response = this.mapper.MapBookingsCollection(bookings);

            return Ok(response);
        }
    }
}