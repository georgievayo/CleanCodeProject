using System.Collections.Generic;

namespace FindAndBook.Models
{
    public class Manager : User
    {
        public Manager() : base()
        {
            this.Restaurants = new HashSet<Restaurant>();
        }

        public virtual ICollection<Restaurant> Restaurants { get; set; }
    }
}
