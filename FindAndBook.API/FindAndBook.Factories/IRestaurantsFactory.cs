using FindAndBook.Models;
using System;

namespace FindAndBook.Factories
{
    public interface IRestaurantsFactory
    {
        Restaurant Create(string name, string contact, string weekendHours,
            string weekdaayHours, string photo, string details, int? averageBill, Guid managerId, string address);
    }
}
