using FindAndBook.Models;
using System;

namespace FindAndBook.Services.Contracts
{
    public interface IUsersService
    {
        User GetById(Guid id);

        User GetByUsername(string username);

        User GetByUsernameAndPassword(string username, string password);

        Manager GetManager(Guid id);

        User GetUser(Guid id);

        User Create(string username, string password, string email, string firstName, 
            string lastName, string phoneNumber, string role);

        bool Delete(Guid id);
    }
}
