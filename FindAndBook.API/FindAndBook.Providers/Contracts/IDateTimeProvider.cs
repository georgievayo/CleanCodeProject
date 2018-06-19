using System;

namespace FindAndBook.Providers.Contracts
{
    public interface IDateTimeProvider
    {
        DateTime GetCurrentTime();
    }
}
