using FindAndBook.Providers.Contracts;
using System;

namespace FindAndBook.Providers
{
    public class DateTimeProvider : IDateTimeProvider
    {
        public DateTime GetCurrentTime()
        {
            return DateTime.UtcNow;
        }
    }
}
