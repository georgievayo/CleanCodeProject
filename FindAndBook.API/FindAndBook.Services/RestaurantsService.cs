﻿using FindAndBook.Data.Contracts;
using FindAndBook.Factories;
using FindAndBook.Models;
using System;
using System.Data.Entity;
using System.Linq;

namespace FindAndBook.Services
{
    public class RestaurantsService
    {
        private readonly IRepository<Restaurant> repository;
        private readonly IUnitOfWork unitOfWork;
        private readonly IRestaurantsFactory factory;

        public RestaurantsService(IRepository<Restaurant> repository, IUnitOfWork unitOfWork, IRestaurantsFactory factory)
        {
            this.repository = repository;
            this.unitOfWork = unitOfWork;
            this.factory = factory;
        }

        public Restaurant Create(string name, string contact, string weekendHours,
            string weekdaayHours, string photo, string details, int? averageBill, string managerId, string address)
        {
            var restaurant = this.factory.Create(name, contact, weekendHours, 
                weekdaayHours, photo, details, averageBill, managerId, address);

            this.repository.Add(restaurant);
            this.unitOfWork.Commit();

            return restaurant;
        }

        public IQueryable<Restaurant> GetAll()
        {
            return this.repository.All;
        }

        public IQueryable<Restaurant> GetById(Guid id)
        {
            return this.repository.All
                .Where(x => x.Id == id);
        }

        public IQueryable<Restaurant> GetUserRestaurants(string userId)
        {
            return this.repository.All
                .Where(x => x.ManagerId == userId)
                .Include(x => x.Bookings);
        }

        public Restaurant Edit(Guid id, string contact, string description,
            string photoUrl, string weekdayHours, string weekendHours, int averageBill)
        {
            var restaurant = this.repository.GetById(id);
            if (restaurant != null)
            {
                restaurant.Contact = contact;
                restaurant.Details = description;
                restaurant.PhotoUrl = photoUrl;
                restaurant.WeekdayHours = weekdayHours;
                restaurant.WeekendHours = weekendHours;
                restaurant.AverageBill = averageBill;

                this.repository.Update(restaurant);
                this.unitOfWork.Commit();
            }

            return restaurant;
        }

        public IQueryable<Restaurant> FindByName(string namePattern)
        {
            return this.repository
                .All
                .Where(x => x.Name.ToLower().Contains(namePattern.ToLower()));
        }

        public IQueryable<Restaurant> FindByAddress(string addressPattern)
        {
            return this.repository
                .All
                .Where(x => x.Address.ToLower().Contains(addressPattern.ToLower()));
        }

        public IQueryable<Restaurant> FindByAverageBill(int averageBill)
        {
            return this.repository
                .All
                .Where(x => x.AverageBill == averageBill);
        }

        public void Delete(Guid id)
        {
            var restaurant = this.repository.GetById(id);

            this.repository.Delete(restaurant);
            this.unitOfWork.Commit();
        }
    }
}
