using FindAndBook.Models;
using System;

namespace FindAndBook.Factories
{
    public interface IBookingsFactory
    {
        Booking Create(Guid restaurantId, Guid userId, DateTime dateTime, int peopleCount);
    }
}
