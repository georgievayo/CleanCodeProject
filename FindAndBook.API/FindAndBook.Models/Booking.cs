using System;
using System.Collections.Generic;

namespace FindAndBook.Models
{
    public class Booking
    {
        public Booking()
        {
            this.Tables = new HashSet<BookedTables>();
        }

        public Booking(Guid? restaurantId, string userId, DateTime dateTime, int people)
            : this()
        {
            RestaurantId = restaurantId;
            UserId = userId;
            DateTime = dateTime;
            NumberOfPeople = people;
        }

        public Guid Id { get; set; }

        public Guid? RestaurantId { get; set; }

        public virtual Restaurant Restaurant { get; set; }

        public string UserId { get; set; }

        public virtual User User { get; set; }

        public DateTime DateTime { get; set; }

        public int NumberOfPeople { get; set; }

        public virtual ICollection<BookedTables> Tables { get; set; }
    }
}
