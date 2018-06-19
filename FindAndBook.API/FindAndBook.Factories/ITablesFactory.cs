using FindAndBook.Models;
using System;

namespace FindAndBook.Factories
{
    public interface ITablesFactory
    {
        Table Create(Guid? placeId, int numberOfPeople, int numberOfTables);
    }
}
