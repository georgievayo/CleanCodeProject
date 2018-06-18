using System;

namespace FindAndBook.Models
{
    public class BookedTables
    {
        public BookedTables()
        {

        }

        public BookedTables(Guid? bookingId, Guid? tableId, int tablesCount)
        {
            this.Id = Guid.NewGuid();
            this.TableId = tableId;
            this.BookingId = bookingId;
            this.TablesCount = tablesCount;
        }

        public Guid? Id { get; set; }

        public Guid? TableId { get; set; }

        public virtual Table Table { get; set; }

        public Guid? BookingId { get; set; }

        public virtual Booking Booking { get; set; }

        public int TablesCount { get; set; }
    }
}
