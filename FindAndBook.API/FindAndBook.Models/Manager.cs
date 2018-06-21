using System.Collections.Generic;

namespace FindAndBook.Models
{
    public class Manager : User
    {
        public Manager() : base()
        {
            this.Role = Role.Manager;
        }

        public Manager(string username, string password, string email,
            string firstName, string lastName, string phoneNumber) 
            : base(username, password, email, firstName, lastName, phoneNumber)
        {
            this.Role = Role.Manager;
            this.Restaurants = new HashSet<Restaurant>();
        }

        public virtual ICollection<Restaurant> Restaurants { get; set; }
    }
}
