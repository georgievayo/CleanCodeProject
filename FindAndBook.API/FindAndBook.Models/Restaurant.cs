using System;
using System.Collections.Generic;

namespace FindAndBook.Models
{
    public class Restaurant
    {
        public Restaurant()
        {
            this.Bookings = new HashSet<Booking>();
            this.Tables = new HashSet<Table>();
        }

        public Restaurant(string name, string contact, string weekendHours,
            string weekdaayHours, string photo, string details, int? averageBill, string managerId, string address)
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
        }

        public Guid Id { get; set; }

        public string Address { get; set; }

        public string ManagerId { get; set; }

        public virtual User Manager { get; set; }

        public string Name { get; set; }

        public string PhotoUrl { get; set; }

        public string Contact { get; set; }

        public string WeekendHours { get; set; }

        public string WeekdayHours { get; set; }

        public string Details { get; set; }

        public int? AverageBill { get; set; }

        public virtual ICollection<Booking> Bookings { get; set; }

        public virtual ICollection<Table> Tables { get; set; }
    }
}
