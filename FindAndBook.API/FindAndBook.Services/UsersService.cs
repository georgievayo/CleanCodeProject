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

        private IRepository<User> usersRepository;

        private IRepository<Manager> managersRepository;

        private IUnitOfWork unitOfWork;

        private IUsersFactory usersFactory;

        private IManagersFactory managerFactory;

        public UsersService(IRepository<User> usersRepository, IRepository<Manager> managersRepository, IUnitOfWork unitOfWork, IUsersFactory usersFactory, IManagersFactory managerFactory)
        {
            this.usersRepository = usersRepository;
            this.managersRepository = managersRepository;
            this.unitOfWork = unitOfWork;
            this.usersFactory = usersFactory;
            this.managerFactory = managerFactory;
        }

        public User GetById(Guid id)
        {
            var foundUser = this.usersRepository.GetById(id);

            return foundUser;
        }

        public User GetByUsername(string username)
        {
            return this.usersRepository
                .All
                .FirstOrDefault(u => u.UserName == username);
        }

        public User GetByUsernameAndPassword(string username, string password)
        {
            return this.usersRepository
                .All
                .FirstOrDefault(u => u.UserName == username && u.Password == password);
        }

        public User GetUser(Guid id)
        {
            return this.usersRepository
                .All
                .Where(x => x.Id == id)
                .Include(x => x.Bookings)
                .FirstOrDefault();
        }

        public Manager GetManager(Guid id)
        {
            return this.managersRepository
                .All
                .Include(m => m.Bookings)
                .Include(m => m.Restaurants)
                .FirstOrDefault(m => m.Id == id);
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
                user = this.usersFactory.Create(username, password, email, firstName, lastName, phoneNumber);
            }

            this.usersRepository.Add(user);
            this.unitOfWork.Commit();

            return user;
        }

        public bool Delete(Guid id)
        {
            var user = this.usersRepository.GetById(id);
            if (user == null)
            {
                return false;
            }

            this.usersRepository.Delete(user);
            this.unitOfWork.Commit();

            return true;
        }
    }
}
