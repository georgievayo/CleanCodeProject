using FindAndBook.Models;
using System;

namespace FindAndBook.Factories
{
    public interface IBookingsFactory
    {
        Booking Create(Guid? placeId, string userId, DateTime dateTime, int people);
    }
}
