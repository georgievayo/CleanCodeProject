using System;
using System.ComponentModel.DataAnnotations;

namespace FindAndBook.API.Models
{
    public class BookingModel
    {
        [Required]
        public DateTime Time { get; set; }

        public Guid RestaurantId { get; set; }

        public Guid UserId { get; set; }

        [Required]
        public int PeopleCount { get; set; }
    }
}