﻿using System;
using System.Collections.Generic;

namespace FindAndBook.Models
{
    public class User
    {
        public User()
        {
            this.Id = Guid.NewGuid();
            this.Role = Role.User;
            this.Bookings = new HashSet<Booking>();
        }

        public User(string username, string password, string email,
            string firstName, string lastName, string phoneNumber)
            : this()
        {
            this.UserName = username;
            this.Password = password;
            this.Email = email;
            this.FirstName = firstName;
            this.LastName = lastName;
            this.PhoneNumber = phoneNumber;
        }

        public Guid Id { get; set; }

        public string UserName { get; set; }

        public string Password { get; set; }

        public string Email { get; set; }

        public string FirstName { get; set; }

        public string LastName { get; set; }

        public string PhoneNumber { get; set; }

        public Role Role { get; set; }

        public virtual ICollection<Booking> Bookings { get; set; }
    }
}
