using FindAndBook.Data.Contracts;
using FindAndBook.Factories;
using FindAndBook.Models;
using FindAndBook.Services.Contracts;
using System;
using System.Data.Entity;
using System.Linq;

namespace FindAndBook.Services
{
    public class UsersService : IUsersService
    {
        private const string MANAGER_ROLE = "manager";

        private IRepository<User> repository;

        private IUnitOfWork unitOfWork;

        private IUsersFactory usersFactory;

        private IManagersFactory managerFactory;

        public UsersService(IRepository<User> repository, IUnitOfWork unitOfWork, IUsersFactory usersFactory, IManagersFactory managerFactory)
        {
            this.repository = repository;
            this.unitOfWork = unitOfWork;
            this.usersFactory = usersFactory;
            this.managerFactory = managerFactory;
        }

        public User GetById(Guid id)
        {
            var foundUser = this.repository.GetById(id);

            return foundUser;
        }

        public User GetByUsername(string username)
        {
            return this.repository
                .All
                .Include(x => x.Bookings)
                .FirstOrDefault(u => u.UserName == username);
        }

        public User GetByUsernameAndPassword(string username, string password)
        {
            return this.repository
                .All
                .FirstOrDefault(u => u.UserName == username && u.Password == password);
        }

        public User GetUserWithBookings(Guid id)
        {
            return this.repository
                .All
                .Where(x => x.Id == id)
                .Include(x => x.Bookings)
                .FirstOrDefault();
        }

        public User Create(string username, string password, string email, string firstName, string lastName, string phoneNumber, string role)
        {
            User user = null;
            if(role.ToLower() == MANAGER_ROLE)
            {
                user = this.managerFactory.Create(username, password, email, firstName, lastName, phoneNumber);
            }
            else
            {
                this.usersFactory.Create(username, password, email, firstName, lastName, phoneNumber);
            }

            this.repository.Add(user);
            this.unitOfWork.Commit();

            return user;
        }

        public IQueryable<User> GetAll()
        {
            return this.repository.All;
        }

        public bool Delete(Guid id)
        {
            var user = this.repository.GetById(id);
            if (user == null)
            {
                return false;
            }

            this.repository.Delete(user);
            this.unitOfWork.Commit();

            return true;
        }
    }
}
