using FindAndBook.Data.Contracts;
using FindAndBook.Factories;
using FindAndBook.Models;
using FindAndBook.Services.Contracts;
using System;
using System.Data.Entity;
using System.Linq;

namespace FindAndBook.Services
{
    public class RestaurantsService : IRestaurantsService
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
            string weekdaayHours, string photo, string details, int? averageBill, 
            Guid managerId, string address, int maxPeopleCount)
        {
            var restaurant = this.factory.Create(name, contact, weekendHours, 
                weekdaayHours, photo, details, averageBill, managerId, address, maxPeopleCount);

            this.repository.Add(restaurant);
            this.unitOfWork.Commit();

            return restaurant;
        }

        public IQueryable<Restaurant> GetAll()
        {
            return this.repository.All;
        }

        public Restaurant GetById(Guid id)
        {
            return this.repository.All
                .FirstOrDefault(x => x.Id == id);
        }

        public IQueryable<Restaurant> GetRestaurantsOfManger(Guid managerId)
        {
            return this.repository.All
                .Where(x => x.ManagerId == managerId)
                .Include(x => x.Bookings);
        }

        public Restaurant Edit(Guid id, string contact, string description,
            string photoUrl, string weekdayHours, string weekendHours, int? averageBill, int maxPeopleCount)
        {
            var restaurant = this.repository.GetById(id);
            if (restaurant != null)
            {
                restaurant.Contact = String.IsNullOrEmpty(contact) ? restaurant.Contact : contact;
                restaurant.Details = String.IsNullOrEmpty(description) ? restaurant.Details : description;
                restaurant.PhotoUrl = String.IsNullOrEmpty(photoUrl) ? restaurant.PhotoUrl : photoUrl;
                restaurant.WeekdayHours = String.IsNullOrEmpty(weekdayHours) ? restaurant.WeekdayHours : weekdayHours;
                restaurant.WeekendHours = String.IsNullOrEmpty(weekendHours) ? restaurant.WeekendHours : weekendHours;
                restaurant.AverageBill = averageBill == null ? restaurant.AverageBill : averageBill;
                restaurant.MaxPeopleCount = maxPeopleCount == 0 ? restaurant.MaxPeopleCount : maxPeopleCount;

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

        public IQueryable<Restaurant> FindBy(string searchBy, string pattern)
        {
            if (searchBy == "Name")
            {
                return this.FindByName(pattern);
            }
            else if (searchBy == "Address")
            {
                return this.FindByAddress(pattern);
            }
            else
            {
                var averageBill = int.Parse(pattern);
                return this.FindByAverageBill(averageBill);
            }
        }

        public bool Delete(Guid id)
        {
            var restaurant = this.repository.GetById(id);
            if(restaurant == null)
            {
                return false;
            }

            this.repository.Delete(restaurant);
            this.unitOfWork.Commit();

            return true;
        }
    }
}
