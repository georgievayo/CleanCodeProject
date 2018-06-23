using System;
using System.Collections.Generic;

namespace FindAndBook.API.Models
{
    public class UserProfileModel
    {
        public Guid Id { get; set; }

        public string Username { get; set; }

        public string Email { get; set; }

        public string FirstName { get; set; }

        public string LastName { get; set; }

        public string PhoneNumber { get; set; }

        public string Role { get; set; }

        public ICollection<BookingModel> Bookings { get; set; }
    }
}