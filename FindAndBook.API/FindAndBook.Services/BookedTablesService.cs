using FindAndBook.Data.Contracts;
using FindAndBook.Factories;
using FindAndBook.Models;
using FindAndBook.Services.Contracts;
using System;
using System.Data.Entity;
using System.Linq;

namespace FindAndBook.Services
{
    public class BookedTablesService : IBookedTablesService
    {
        private readonly IRepository<BookedTables> repository;
        private readonly IUnitOfWork unitOfWork;
        private readonly IBookedTablesFactory factory;

        public BookedTablesService(IRepository<BookedTables> repository,
            IUnitOfWork unitOfWork, IBookedTablesFactory factory)
        {
            this.repository = repository;
            this.unitOfWork = unitOfWork;
            this.factory = factory;
        }
        public void BookTable(Guid bookingId, Guid tableId, int tablesCount)
        {
            var bookedTable = this.factory.CreateBookedTable(bookingId, tableId, tablesCount);

            this.repository.Add(bookedTable);
            this.unitOfWork.Commit();
        }

        public BookedTables GetAllByBooking(Guid bookingId)
        {
            return this.repository
                .All
                .Include(x => x.Table)
                .FirstOrDefault(x => x.BookingId == bookingId);
        }

        public void DeleteAllByBooking(Guid bookingId)
        {
            var bookedTables = this.repository
                .All
                .Where(x => x.BookingId == bookingId);

            foreach (var bookedTable in bookedTables)
            {
                this.repository.Delete(bookedTable);
                this.unitOfWork.Commit();
            }
        }
    }
}
