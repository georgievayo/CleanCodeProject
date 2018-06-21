using System.Collections.Generic;
using System.Linq;
using FindAndBook.API.Models;
using FindAndBook.Models;

namespace FindAndBook.API.Mapper
{
    public class ModelsMapper : IModelsMapper
    {
        public BookingModel MapBooking(Booking booking)
        {
            var rest = booking.Restaurant.Name;
            var user = booking.User.FirstName + " " + booking.User.LastName;

            var mappedBooking = new BookingModel()
            {
                Restaurant = booking.Restaurant.Name,
                User = booking.User.FirstName + " " + booking.User.LastName,
                Time = booking.DateTime,
                PeopleCount = booking.PeopleCount
            };

            return mappedBooking;
        }

        public RestaurantModel MapRestaurant(Restaurant restaurant)
        {
            var mappedRestaurant = new RestaurantModel()
            {
                Id = restaurant.Id,
                Name = restaurant.Name,
                Details = restaurant.Details,
                Contact = restaurant.Contact,
                WeekendHours = restaurant.WeekendHours,
                WeekdayHours = restaurant.WeekdayHours,
                Address = restaurant.Address,
                PhotoUrl = restaurant.PhotoUrl,
                AverageBill = restaurant.AverageBill,
                MaxPeopleCount = restaurant.MaxPeopleCount,
                Manager = restaurant.Manager.FirstName + " " + restaurant.Manager.LastName
            };

            return mappedRestaurant;
        }

        public List<RestaurantModel> MapRestaurantsCollection(IQueryable<Restaurant> restaurants)
        {
            var mappedRestaurantsCollection = new List<RestaurantModel>();

            foreach (var restaurant in restaurants)
            {
                var mappedRestaurant = this.MapRestaurant(restaurant);
                mappedRestaurantsCollection.Add(mappedRestaurant);
            }

            return mappedRestaurantsCollection;
        }
    }
}