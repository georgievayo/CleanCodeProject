using FindAndBook.Models;
using System;

namespace FindAndBook.Services.Contracts
{
    public interface ITablesService
    {
        Table CreateTableType(Guid placeId, int numberOfPeople, int numberOfTables);

        int GetTablesCount(Guid restaurantId, int peopleCount);

        Table GetByRestaurantAndPeopleCount(Guid restaurantId, int peopleCount);
    }
}
