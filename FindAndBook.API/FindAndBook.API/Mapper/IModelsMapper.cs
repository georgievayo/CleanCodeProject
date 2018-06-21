using FindAndBook.API.Models;
using FindAndBook.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FindAndBook.API.Mapper
{
    public interface IModelsMapper
    {
        List<RestaurantModel> MapRestaurantsCollection(IQueryable<Restaurant> restaurants);

        RestaurantModel MapRestaurant(Restaurant restaurant);


    }
}
