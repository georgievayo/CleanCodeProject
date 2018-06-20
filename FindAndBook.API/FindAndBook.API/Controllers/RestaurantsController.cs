using FindAndBook.API.Models;
using FindAndBook.Providers.Contracts;
using FindAndBook.Services.Contracts;
using Newtonsoft.Json;
using System;
using System.Web.Http;

namespace FindAndBook.API.Controllers
{
    [RoutePrefix("api/restaurants")]
    public class RestaurantsController : ApiController
    {
        private readonly IAuthenticationProvider authProvider;

        private readonly IRestaurantsService restaurantsService;

        public RestaurantsController(IAuthenticationProvider authProvider, IRestaurantsService restaurantsService)
        {
            this.authProvider = authProvider;
            this.restaurantsService = restaurantsService;
        }

        [HttpPost]
        public IHttpActionResult CreateRestaurant(RestaurantModel model)
        {
            if(model == null || !ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var currentUserId = this.authProvider.CurrentUserID;

            var createdRestaurant = this.restaurantsService.Create(model.Name, model.Contact, model.WeekendHours, 
                model.WeekdayHours, model.PhotoUrl, model.Details, model.AverageBill, Guid.Parse(currentUserId), model.Address);

            var response = new
            {
                Id = createdRestaurant.Id,
                Name = createdRestaurant.Name,
                Details = createdRestaurant.Details
            };

            return Ok(response);
        }

        [HttpGet]
        [Route("{id}")]
        public IHttpActionResult GetRestaurantDetails([FromUri]string id)
        {
            if(id == null || String.IsNullOrEmpty(id))
            {
                return BadRequest();
            }

            try
            {
                var restaurantId = Guid.Parse(id);
                var restaurant = this.restaurantsService.GetById(restaurantId);

                if(restaurant == null)
                {
                    return NotFound();
                }
                else
                {
                    var response = new
                    {
                        Id = restaurant.Id,
                        Name = restaurant.Name,
                        Details = restaurant.Details,
                        Contact = restaurant.Contact,
                        WeekendHours = restaurant.WeekendHours,
                        WeekdayHours = restaurant.WeekdayHours,
                        Address = restaurant.Address,
                        AverageBill = restaurant.AverageBill,
                        Manager = restaurant.Manager.FirstName + " " + restaurant.Manager.LastName
                    };

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