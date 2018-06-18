using FindAndBook.Models;
using System;

namespace FindAndBook.Factories
{
    interface ITablesFactory
    {
        Table Create(Guid? placeId, int numberOfPeople, int numberOfTables);
    }
}
