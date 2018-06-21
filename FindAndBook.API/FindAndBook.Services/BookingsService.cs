using FindAndBook.Data.Contracts;
using FindAndBook.Factories;
using FindAndBook.Models;
using FindAndBook.Services.Contracts;
using System;
using System.Data.Entity;
using System.Linq;

namespace FindAndBook.Services
{
    public class BookingsService : IBookingsService
    {
        private readonly IRepository<Booking> repository;

        private readonly IUnitOfWork unitOfWork;

        private readonly IBookingsFactory factory;

        public BookingsService(IRepository<Booking> repository, IUnitOfWork unitOfWork, IBookingsFactory factory)
        {
            this.repository = repository;
            this.unitOfWork = unitOfWork;
            this.factory = factory;
        }

        public IQueryable<Booking> GetAllOfRestaurant(Guid restaurantId)
        {
            return this.repository
                .All
                .Where(x => x.RestaurantId == restaurantId);
        }

        public IQueryable<Booking> GetAllOn(DateTime dateTime, Guid restaurantId)
        {
            return this.repository
                .All
                .Where(x => x.DateTime == dateTime && x.RestaurantId == restaurantId);
        }

        public Booking GetById(Guid id)
        {
            return this.repository.GetById(id);
        }

        public Booking Create(Guid restaurantId, Guid userId, DateTime dateTime, int people)
        {
            var booking = this.factory.Create(restaurantId, userId, dateTime, people);

            this.repository.Add(booking);
            this.unitOfWork.Commit();

            return booking;
        }

        public void Delete(Guid id)
        {
            var booking = this.repository.GetById(id);

            this.repository.Delete(booking);
            this.unitOfWork.Commit();
        }
    }
}
