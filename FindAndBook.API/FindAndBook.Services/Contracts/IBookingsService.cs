using FindAndBook.Models;
using System;
using System.Linq;

namespace FindAndBook.Services.Contracts
{
    public interface IBookingsService
    {
        IQueryable<Booking> GetAllOfRestaurant(Guid restaurantId);

        IQueryable<Booking> GetAllOn(DateTime dateTime, Guid restaurantId);

        Booking GetById(Guid id);

        Booking Create(Guid restaurantId, Guid userId, DateTime dateTime, int peopleCount);

        Booking BookTable(Guid restaurantId, Guid userId, DateTime dateTime, int peopleCount);

        void Delete(Booking booking);
    }
}
