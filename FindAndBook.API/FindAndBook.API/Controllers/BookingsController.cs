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
        public IHttpActionResult BookATable([FromUri] string id, BookingModel model)
        {
            if(model == null || !ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            try
            {
                var restaurantId = Guid.Parse(id);
                var userId = this.authProvider.CurrentUserID;

                var booking = this.bookingsService.BookTable(restaurantId, userId, model.Time, model.PeopleCount);
                if(booking == null)
                {
                    return Conflict();
                }
                else
                {
                    var response = this.mapper.MapBooking(booking);

                    return Ok(response);
                }
            }
            catch(FormatException)
            {
                return BadRequest("Restaurant id is incorrect.");
            }
        }

        [HttpDelete]
        [Route("api/bookings/{id}")]
        public IHttpActionResult CancelBooking([FromUri] string id)
        {
            if(String.IsNullOrEmpty(id))
            {
                return BadRequest("Booking Id is required.");
            }

            try
            {
                var bookingId = Guid.Parse(id);

                var booking = this.bookingsService.GetById(bookingId);
                if(booking == null)
                {
                    return NotFound();
                }
                else
                {
                    var currentUserId = this.authProvider.CurrentUserID;
                    if(booking.UserId != currentUserId)
                    {
                        return Content(System.Net.HttpStatusCode.Forbidden, "You can cancel only your bookings.");
                    }
                    else
                    {
                        this.bookingsService.Delete(booking);
                        return Ok();
                    }
                }
            }
            catch (FormatException)
            {
                return BadRequest("Booking Id is incorrect.");
            }
        }

        [HttpGet]
        [Route("api/restaurants/{id}/bookings")]
        public IHttpActionResult GetAll([FromUri] string id, [FromUri] BookingCriteria criteria)
        {
            if (String.IsNullOrEmpty(id))
            {
                return BadRequest("Restaurant id is required.");
            }

            try
            {
                var restaurantId = Guid.Parse(id);
                IQueryable<Booking> bookings = null;

                if(criteria.Time == null)
                {
                    bookings = this.bookingsService.GetAllOfRestaurant(restaurantId);
                }
                else
                {
                    var time = (DateTime)criteria.Time;
                    bookings = this.bookingsService.GetAllOn(time, restaurantId);
                }

                var response = this.mapper.MapBookingsCollection(bookings);

                return Ok(response);
            }
            catch (FormatException)
            {
                return BadRequest("Restaurant Id is incorrect.");
            }
        }
    }
}