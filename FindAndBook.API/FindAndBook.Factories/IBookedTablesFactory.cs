using FindAndBook.Models;
using System;

namespace FindAndBook.Factories
{
    public interface IBookedTablesFactory
    {
        BookedTables CreateBookedTable(Guid? bookingId, Guid? tableId, int tablesCount);
    }
}
