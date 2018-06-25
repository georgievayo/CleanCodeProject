using System.Collections.Generic;

namespace FindAndBook.API.Models
{
    public class ManagerProfileModel : UserProfileModel
    {
        public ICollection<RestaurantModel> Restaurants { get; set; }
    }
}