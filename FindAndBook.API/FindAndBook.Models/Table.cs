using System;
using System.Collections.Generic;

namespace FindAndBook.Models
{
    public class Table
    {
        public Table()
        {
            this.Bookings = new HashSet<BookedTables>();
        }

        public Table(Guid? restaurantId, int numberOfPeople, int numberOfTables)
            : this()
        {
            this.RestaurantId = restaurantId;
            this.NumberOfPeople = numberOfPeople;
            this.NumberOfTables = numberOfTables;
        }

        public Guid Id { get; set; }

        public Guid? RestaurantId { get; set; }

        public virtual Restaurant Restaurant { get; set; }

        public int NumberOfPeople { get; set; }

        public int NumberOfTables { get; set; }

        public virtual ICollection<BookedTables> Bookings { get; set; }
    }
}
