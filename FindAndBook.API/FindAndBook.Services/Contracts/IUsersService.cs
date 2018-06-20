using FindAndBook.Models;
using System;
using System.Linq;

namespace FindAndBook.Services.Contracts
{
    public interface IUsersService
    {
        User GetById(Guid id);

        User GetByUsername(string username);

        User GetByUsernameAndPassword(string username, string password);

        User GetUserWithBookings(Guid id);

        User Create(string username, string password, string email, string firstName, string lastName, string phoneNumber);

        IQueryable<User> GetAll();

        void Delete(Guid id);
    }
}
