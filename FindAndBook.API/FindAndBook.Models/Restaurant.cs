using System;
using System.Collections.Generic;

namespace FindAndBook.Models
{
    public class Restaurant
    {
        public Restaurant()
        {
            this.Id = Guid.NewGuid();
            this.Bookings = new HashSet<Booking>();
        }

        public Restaurant(string name, string contact, string weekendHours,
            string weekdaayHours, string photo, string details, int? averageBill, 
            Guid managerId, string address, int maxPeopleCount)
            : this()
        {
            this.Name = name;
            this.PhotoUrl = photo;
            this.Contact = contact;
            this.WeekdayHours = weekdaayHours;
            this.WeekendHours = weekendHours;
            this.Details = details;
            this.AverageBill = averageBill;
            this.ManagerId = managerId;
            this.Address = address;
            this.MaxPeopleCount = maxPeopleCount;
        }

        public Guid Id { get; set; }

        public string Address { get; set; }

        public Guid ManagerId { get; set; }

        public virtual Manager Manager { get; set; }

        public string Name { get; set; }

        public string PhotoUrl { get; set; }

        public string Contact { get; set; }

        public string WeekendHours { get; set; }

        public string WeekdayHours { get; set; }

        public string Details { get; set; }

        public int? AverageBill { get; set; }

        public int MaxPeopleCount { get; set; }

        public virtual ICollection<Booking> Bookings { get; set; }
    }
}
