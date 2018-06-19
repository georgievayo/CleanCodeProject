using FindAndBook.Models;
using System.Linq;

namespace FindAndBook.Services.Contracts
{
    public interface IUsersService
    {
        User GetById(string id);

        IQueryable<User> GetByUsername(string username);

        User GetUserWithBookings(string id);

        User Create(string username, string email, string firstName, string lastName, string phoneNumber);

        IQueryable<User> GetAll();

        void Delete(string id);
    }
}
