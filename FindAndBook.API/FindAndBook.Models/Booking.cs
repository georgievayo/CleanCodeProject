using System;

namespace FindAndBook.Models
{
    public class Booking
    {
        public Booking()
        {
            this.Id = Guid.NewGuid();
        }

        public Booking(Guid restaurantId, Guid userId, DateTime dateTime, int peopleCount)
            : this()
        {
            RestaurantId = restaurantId;
            UserId = userId;
            DateTime = dateTime;
            PeopleCount = peopleCount;
        }

        public Guid Id { get; set; }

        public Guid RestaurantId { get; set; }

        public virtual Restaurant Restaurant { get; set; }

        public Guid UserId { get; set; }

        public virtual User User { get; set; }

        public DateTime DateTime { get; set; }

        public int PeopleCount { get; set; }
    }
}
