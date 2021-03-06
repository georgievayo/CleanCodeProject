﻿using System.Collections.Generic;
using System.Linq;
using FindAndBook.API.Models;
using FindAndBook.Models;

namespace FindAndBook.API.Mapper
{
    public class ModelsMapper : IModelsMapper
    {
        public BookingModel MapBooking(Booking booking)
        {
            var mappedBooking = new BookingModel()
            {
                RestaurantId = booking.RestaurantId,
                UserId = booking.UserId,
                Time = booking.DateTime,
                PeopleCount = booking.PeopleCount
            };

            return mappedBooking;
        }

        public List<BookingModel> MapBookingsCollection(IQueryable<Booking> bookings)
        {
            var mappedBookingsCollection = new List<BookingModel>();

            foreach (var booking in bookings)
            {
                var mappedBooking = this.MapBooking(booking);
                mappedBookingsCollection.Add(mappedBooking);
            }

            return mappedBookingsCollection;
        }

        public ManagerProfileModel MapManager(Manager manager)
        {
            var queryableBookings = manager.Bookings.AsQueryable();
            var queryableRestaurants = manager.Restaurants.AsQueryable();

            var mappedManager = new ManagerProfileModel()
            {
                Username = manager.UserName,
                Id = manager.Id,
                Email = manager.Email,
                Role = manager.Role.ToString(),
                FirstName = manager.FirstName,
                LastName = manager.LastName,
                Bookings = this.MapBookingsCollection(queryableBookings),
                Restaurants = this.MapRestaurantsCollection(queryableRestaurants)
            };

            return mappedManager;
        }

        public RestaurantModel MapRestaurant(Restaurant restaurant)
        {
            var mappedRestaurant = new RestaurantModel()
            {
                Id = restaurant.Id,
                Name = restaurant.Name,
                Details = restaurant.Details,
                Contact = restaurant.Contact,
                WeekendHours = restaurant.WeekendHours,
                WeekdayHours = restaurant.WeekdayHours,
                Address = restaurant.Address,
                PhotoUrl = restaurant.PhotoUrl,
                AverageBill = restaurant.AverageBill,
                MaxPeopleCount = restaurant.MaxPeopleCount,
                Manager = restaurant.Manager.FirstName + " " + restaurant.Manager.LastName
            };

            return mappedRestaurant;
        }

        public List<RestaurantModel> MapRestaurantsCollection(IQueryable<Restaurant> restaurants)
        {
            var mappedRestaurantsCollection = new List<RestaurantModel>();

            foreach (var restaurant in restaurants)
            {
                var mappedRestaurant = this.MapRestaurant(restaurant);
                mappedRestaurantsCollection.Add(mappedRestaurant);
            }

            return mappedRestaurantsCollection;
        }

        public UserProfileModel MapUser(User user)
        {
            var queryableBookings = user.Bookings.AsQueryable();

            var mappedUser = new UserProfileModel()
            {
                Username = user.UserName,
                Id = user.Id,
                Email = user.Email,
                Role = user.Role.ToString(),
                FirstName = user.FirstName,
                LastName = user.LastName,
                Bookings = this.MapBookingsCollection(queryableBookings)
            };

            return mappedUser;
        }
    }
}