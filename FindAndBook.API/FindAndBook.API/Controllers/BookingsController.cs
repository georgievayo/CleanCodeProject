using FindAndBook.API.Mapper;
using FindAndBook.API.Models;
using FindAndBook.Providers.Contracts;
using FindAndBook.Services.Contracts;
using System;
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
    }
}