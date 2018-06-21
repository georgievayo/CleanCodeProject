using FindAndBook.API.Models;
using FindAndBook.Models;
using System.Collections.Generic;
using System.Linq;

namespace FindAndBook.API.Mapper
{
    public interface IModelsMapper
    {
        List<RestaurantModel> MapRestaurantsCollection(IQueryable<Restaurant> restaurants);

        RestaurantModel MapRestaurant(Restaurant restaurant);

        BookingModel MapBooking(Booking booking);
    }
}
