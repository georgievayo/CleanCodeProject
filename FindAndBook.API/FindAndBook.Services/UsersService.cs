using FindAndBook.Data.Contracts;
using FindAndBook.Factories;
using FindAndBook.Models;
using FindAndBook.Services.Contracts;
using System.Data.Entity;
using System.Linq;

namespace FindAndBook.Services
{
    public class UsersService : IUsersService
    {
        private IRepository<User> repository;

        private IUnitOfWork unitOfWork;

        private IUsersFactory usersFactory;

        public UsersService(IRepository<User> repository, IUnitOfWork unitOfWork, IUsersFactory usersFactory)
        {
            this.repository = repository;
            this.unitOfWork = unitOfWork;
            this.usersFactory = usersFactory;
        }

        public User GetById(string id)
        {
            var foundUser = this.repository.GetById(id);

            return foundUser;
        }

        public IQueryable<User> GetByUsername(string username)
        {
            return this.repository
                .All
                .Where(u => u.UserName == username)
                .Include(x => x.Bookings);
        }

        public User GetUserWithBookings(string id)
        {
            return this.repository
                .All
                .Where(x => x.Id == id)
                .Include(x => x.Bookings)
                .FirstOrDefault();
        }

        public User Create(string username, string email, string firstName, string lastName, string phoneNumber)
        {
            var user = this.usersFactory.Create(username, email, firstName, lastName, phoneNumber);

            return user;
        }

        public IQueryable<User> GetAll()
        {
            return this.repository.All;
        }

        public void Delete(string id)
        {
            var user = this.repository.GetById(id);

            this.repository.Delete(user);
            this.unitOfWork.Commit();
        }
    }
}
