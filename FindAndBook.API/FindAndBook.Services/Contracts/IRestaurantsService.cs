using FindAndBook.Models;
using System;
using System.Linq;

namespace FindAndBook.Services.Contracts
{
    public interface IRestaurantsService
    {
        Restaurant Create(string name, string contact, string weekendHours, string weekdaayHours, 
            string photo, string details, int? averageBill, Guid userId, string address);

        IQueryable<Restaurant> GetAll();

        Restaurant GetById(Guid id);

        IQueryable<Restaurant> GetUserRestaurants(Guid userId);

        Restaurant Edit(Guid id, string contact, string description, string photoUrl,
            string weekdayHours, string weekendHours, int averageBill);

        IQueryable<Restaurant> FindByName(string pattern);

        IQueryable<Restaurant> FindByAddress(string pattern);

        IQueryable<Restaurant> FindByAverageBill(int averageBill);

        IQueryable<Restaurant> FindBy(string searchBy, string pattern);

        void Delete(Guid id);
    }
}
