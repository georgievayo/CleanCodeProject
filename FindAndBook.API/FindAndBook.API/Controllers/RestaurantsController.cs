using FindAndBook.API.Mapper;
using FindAndBook.API.Models;
using FindAndBook.Providers.Contracts;
using FindAndBook.Services.Contracts;
using System;
using System.Web.Http;

namespace FindAndBook.API.Controllers
{
    public class RestaurantsController : ApiController
    {
        private const string MANAGER_ROLE = "Manager";

        private readonly IAuthenticationProvider authProvider;

        private readonly IRestaurantsService restaurantsService;

        private readonly IModelsMapper mapper;

        public RestaurantsController(IAuthenticationProvider authProvider, IRestaurantsService restaurantsService,
            IModelsMapper mapper)
        {
            this.authProvider = authProvider;
            this.restaurantsService = restaurantsService;
            this.mapper = mapper;
        }

        [HttpPost]
        [Route("api/restaurants")]
        public IHttpActionResult CreateRestaurant(RestaurantModel model)
        {
            if (model == null || !ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var currentUserRole = this.authProvider.CurrentUserRole;

            if (currentUserRole == MANAGER_ROLE)
            {
                var currentUserId = this.authProvider.CurrentUserID;

                var createdRestaurant = this.restaurantsService.Create(model.Name, model.Contact, model.WeekendHours,
                model.WeekdayHours, model.PhotoUrl, model.Details, model.AverageBill, currentUserId, model.Address, model.MaxPeopleCount);

                var response = this.mapper.MapRestaurant(createdRestaurant);

                return Ok(response);
            }
            else
            {
                return Content(System.Net.HttpStatusCode.Forbidden, "Only managers can create a restaurant.");
            }
        }

        [HttpGet]
        [Route("api/restaurants/{id}")]
        public IHttpActionResult GetRestaurantDetails([FromUri]Guid? id)
        {
            if (id == null)
            {
                return BadRequest();
            }
            var restaurant = this.restaurantsService.GetById((Guid)id);

            if (restaurant == null)
            {
                return NotFound();
            }
            else
            {
                var response = this.mapper.MapRestaurant(restaurant);

                return Ok(response);
            }
        }

        [HttpGet]
        [Route("api/restaurants")]
        public IHttpActionResult Search([FromUri] SearchCriteria criteria)
        {
            if (criteria == null || (criteria.SearchBy == null && criteria.Pattern == null))
            {
                var allRestaurants = this.restaurantsService.GetAll();
                var response = this.mapper.MapRestaurantsCollection(allRestaurants);

                return Ok(response);
            }

            try
            {
                var foundRestaurants = this.restaurantsService
                    .FindBy(criteria.SearchBy, criteria.Pattern);

                var response = this.mapper.MapRestaurantsCollection(foundRestaurants);

                return Ok(response);
            }
            catch (FormatException)
            {
                return BadRequest("Average Bill must be a valid number.");
            }
        }

        [HttpGet]
        [Route("api/users/{id}/restaurants")]
        public IHttpActionResult GetManagerRestaurants([FromUri]Guid? id)
        {
            if (id == null)
            {
                return BadRequest();
            }

            var currentUserId = this.authProvider.CurrentUserID;

            if (id != currentUserId)
            {
                return Content(System.Net.HttpStatusCode.Forbidden, "You can see only list of your restaurants");
            }

            var restaurants = this.restaurantsService.GetRestaurantsOfManger((Guid)id);

            var response = this.mapper.MapRestaurantsCollection(restaurants);

            return Ok(response);
        }

        [HttpPut]
        [Route("api/restaurants/{id}")]
        public IHttpActionResult EditRestaurant([FromUri] Guid? id, RestaurantModel model)
        {
            if (id == null)
            {
                return BadRequest("Restaurant id is required.");
            }

            if (model == null || !ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var restaurant = this.restaurantsService.GetById((Guid)id);
            if (restaurant == null)
            {
                return NotFound();
            }

            var currentUserId = this.authProvider.CurrentUserID;

            if (currentUserId == restaurant.ManagerId)
            {
                var updatedRestaurant = this.restaurantsService.Edit((Guid)id, model.Contact,
                    model.Details, model.PhotoUrl, model.WeekdayHours, model.WeekendHours,
                    model.AverageBill, model.MaxPeopleCount);

                var response = this.mapper.MapRestaurant(updatedRestaurant);

                return Ok(response);
            }
            else
            {
                return Content(System.Net.HttpStatusCode.Forbidden, "Only manager of this restaurant can edit it.");
            }
        }

        [HttpDelete]
        [Route("api/restaurants/{id}")]
        public IHttpActionResult DeleteRestaurant([FromUri] Guid? id)
        {
            if (id == null)
            {
                return BadRequest("Restaurant Id is required.");
            }

            var restaurant = this.restaurantsService.GetById((Guid)id);
            if (restaurant == null)
            {
                return NotFound();
            }
            var currentUserId = this.authProvider.CurrentUserID;

            if (currentUserId == restaurant.ManagerId)
            {
                this.restaurantsService.Delete((Guid)id);

                return Ok();
            }
            else
            {
                return Content(System.Net.HttpStatusCode.Forbidden, "Only manager of this restaurant can delete it.");
            }
        }
    }
}