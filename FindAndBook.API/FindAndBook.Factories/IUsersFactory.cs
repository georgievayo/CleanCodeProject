using FindAndBook.Models;

namespace FindAndBook.Factories
{
    public interface IUsersFactory
    {
        User Create(string username, string password, string email, 
            string firstName, string lastName, string phoneNumber);
    }
}
