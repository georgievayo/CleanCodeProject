using FindAndBook.Data.Contracts;
using FindAndBook.Factories;
using FindAndBook.Models;
using FindAndBook.Services.Contracts;
using System;
using System.Linq;

namespace FindAndBook.Services
{
    public class TablesService : ITablesService
    {
        private readonly IRepository<Table> repository;
        private readonly IUnitOfWork unitOfWork;
        private readonly ITablesFactory factory;

        public TablesService(IRepository<Table> repository, IUnitOfWork unitOfWork, ITablesFactory factory)
        {
            this.repository = repository;
            this.unitOfWork = unitOfWork;
            this.factory = factory;
        }

        public Table CreateTableType(Guid placeId, int numberOfPeople, int numberOfTables)
        {
            var tableType = this.factory.Create(placeId, numberOfPeople, numberOfTables);

            this.repository.Add(tableType);
            this.unitOfWork.Commit();

            return tableType;
        }

        public int GetTablesCount(Guid restaurantId, int peopleCount)
        {
            var tableType = this.GetByRestaurantAndPeopleCount(restaurantId, peopleCount);

            if (tableType == null)
            {
                return 0;
            }

            return tableType.NumberOfTables;
        }

        public Table GetByRestaurantAndPeopleCount(Guid restaurantId, int peopleCount)
        {
            return this.repository
                .All
                .FirstOrDefault(x => x.RestaurantId == restaurantId && x.NumberOfPeople == peopleCount);
        }
    }
}
