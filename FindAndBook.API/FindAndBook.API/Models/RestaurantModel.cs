using System;

namespace FindAndBook.API.Models
{
    public class RestaurantModel
    {
        public Guid Id { get; set; }

        public string Address { get; set; }

        public string Manager { get; set; }

        public string Name { get; set; }

        public string PhotoUrl { get; set; }

        public string Contact { get; set; }

        public string WeekendHours { get; set; }

        public string WeekdayHours { get; set; }

        public string Details { get; set; }

        public int? AverageBill { get; set; }

        public int MaxPeopleCount { get; set; }
    }
}