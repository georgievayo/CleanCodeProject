using System;
using System.ComponentModel.DataAnnotations;

namespace FindAndBook.API.Models
{
    public class BookingModel
    {
        [Required]
        public DateTime Time { get; set; }

        public string Restaurant { get; set; }

        public string User { get; set; }

        [Required]
        public int PeopleCount { get; set; }
    }
}