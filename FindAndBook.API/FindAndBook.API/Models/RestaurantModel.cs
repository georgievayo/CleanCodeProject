using System;
using System.ComponentModel.DataAnnotations;

namespace FindAndBook.API.Models
{
    public class RestaurantModel
    {
        public Guid Id { get; set; }

        [Required]
        public string Address { get; set; }

        public string Manager { get; set; }

        [Required]
        public string Name { get; set; }

        public string PhotoUrl { get; set; }

        [Required]
        public string Contact { get; set; }

        [Required]
        public string WeekendHours { get; set; }

        [Required]
        public string WeekdayHours { get; set; }

        public string Details { get; set; }

        [RegularExpression("^[0-9]+$", ErrorMessage = "Average Bill must be a valid number.")]
        public int? AverageBill { get; set; }

        public int MaxPeopleCount { get; set; }
    }
}