using FindAndBook.Models;
using System;

namespace FindAndBook.Services.Contracts
{
    public interface IBookedTablesService
    {
        void BookTable(Guid bookingId, Guid tableId, int tablesCount);

        BookedTables GetAllByBooking(Guid bookingId);

        void DeleteAllByBooking(Guid bookingId);
    }
}
