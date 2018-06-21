using FindAndBook.Models;

namespace FindAndBook.Factories
{
    public interface IManagersFactory
    {
        Manager Create(string username, string password, string email,
            string firstName, string lastName, string phoneNumber);
    }
}
