using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using System.Collections.Generic;

namespace FindAndBook.Models
{
    public class User : IdentityUser, IUser
    {
        public User()
        {
            this.Bookings = new HashSet<Booking>();
            this.Restaurants = new HashSet<Restaurant>();
        }

        public User(string username, string email, string firstName, string lastName, string phoneNumber)
            : this()
        {
            this.UserName = username;
            this.Email = email;
            this.FirstName = firstName;
            this.LastName = lastName;
            this.PhoneNumber = phoneNumber;
        }

        public string FirstName { get; set; }

        public string LastName { get; set; }

        public virtual ICollection<Booking> Bookings { get; set; }

        public virtual ICollection<Restaurant> Restaurants { get; set; }
    }
}
