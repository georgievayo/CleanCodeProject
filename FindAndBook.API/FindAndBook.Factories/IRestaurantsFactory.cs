using FindAndBook.Models;

namespace FindAndBook.Factories
{
    public interface IRestaurantsFactory
    {
        Restaurant Create(string name, string contact, string weekendHours,
            string weekdaayHours, string photo, string details, int? averageBill, string managerId, string address);
    }
}
