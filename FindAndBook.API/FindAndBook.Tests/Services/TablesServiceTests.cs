using FindAndBook.Data.Contracts;
using FindAndBook.Factories;
using FindAndBook.Models;
using FindAndBook.Services;
using Moq;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FindAndBook.Tests.Services
{
    [TestFixture]
    public class TablesServiceTests
    {
        [TestCase("d547a40d-c45f-4c43-99de-0bfe9199ff95", 2, 5)]
        [TestCase("d547a40d-c45f-4c43-99de-0bfe9199ff95", 4, 5)]
        [TestCase("d547a40d-c45f-4c43-99de-0bfe9199ff95", 6, 5)]
        public void MethodCreateTableTypeShould_CallFactoryMethodCreate(string restaurantId, int numberOfPeople, int numberOfTables)
        {
            var repositoryMock = new Mock<IRepository<Table>>();
            var unitOfWorkMock = new Mock<IUnitOfWork>();
            var factoryMock = new Mock<ITablesFactory>();

            var service = new TablesService(repositoryMock.Object, unitOfWorkMock.Object, factoryMock.Object);
            var restaurantIdGuid = new Guid(restaurantId);

            service.CreateTableType(restaurantIdGuid, numberOfPeople, numberOfTables);

            factoryMock.Verify(f => f.Create(restaurantIdGuid, numberOfPeople, numberOfTables), Times.Once);
        }

        [TestCase("d547a40d-c45f-4c43-99de-0bfe9199ff95", 2, 5)]
        [TestCase("d547a40d-c45f-4c43-99de-0bfe9199ff95", 4, 5)]
        [TestCase("d547a40d-c45f-4c43-99de-0bfe9199ff95", 6, 5)]
        public void MethodCreateTableTypeShould_CallRepositoryMethodAdd(string restaurantId, int numberOfPeople, int numberOfTables)
        {
            var repositoryMock = new Mock<IRepository<Table>>();
            var unitOfWorkMock = new Mock<IUnitOfWork>();
            var factoryMock = new Mock<ITablesFactory>();

            var service = new TablesService(repositoryMock.Object, unitOfWorkMock.Object, factoryMock.Object);
            var restaurantIdGuid = new Guid(restaurantId);
            var table = new Table()
            {
                RestaurantId = restaurantIdGuid,
                NumberOfPeople = numberOfPeople,
                NumberOfTables = numberOfTables
            };
            factoryMock.Setup(x => x.Create(restaurantIdGuid, numberOfPeople, numberOfTables)).Returns(table);
            service.CreateTableType(restaurantIdGuid, numberOfPeople, numberOfTables);

            repositoryMock.Verify(f => f.Add(table), Times.Once);
        }

        [TestCase("d547a40d-c45f-4c43-99de-0bfe9199ff95", 2, 5)]
        [TestCase("d547a40d-c45f-4c43-99de-0bfe9199ff95", 4, 5)]
        [TestCase("d547a40d-c45f-4c43-99de-0bfe9199ff95", 6, 5)]
        public void MethodCreateTableTypeShould_CallUnitOfWorkMethodCommit(string restaurantId, int numberOfPeople, int numberOfTables)
        {
            var repositoryMock = new Mock<IRepository<Table>>();
            var unitOfWorkMock = new Mock<IUnitOfWork>();
            var factoryMock = new Mock<ITablesFactory>();

            var service = new TablesService(repositoryMock.Object, unitOfWorkMock.Object, factoryMock.Object);
            var restaurantIdGuid = new Guid(restaurantId);
            var table = new Table()
            {
                RestaurantId = restaurantIdGuid,
                NumberOfPeople = numberOfPeople,
                NumberOfTables = numberOfTables
            };

            factoryMock.Setup(x => x.Create(restaurantIdGuid, numberOfPeople, numberOfTables)).Returns(table);
            service.CreateTableType(restaurantIdGuid, numberOfPeople, numberOfTables);

            unitOfWorkMock.Verify(f => f.Commit(), Times.Once);
        }

        [TestCase("d547a40d-c45f-4c43-99de-0bfe9199ff95", 2, 5)]
        [TestCase("d547a40d-c45f-4c43-99de-0bfe9199ff95", 4, 5)]
        [TestCase("d547a40d-c45f-4c43-99de-0bfe9199ff95", 6, 5)]
        public void MethodCreateTableTypeShould_ReturnCorrectTableType(string restaurantId, int numberOfPeople, int numberOfTables)
        {
            var repositoryMock = new Mock<IRepository<Table>>();
            var unitOfWorkMock = new Mock<IUnitOfWork>();
            var factoryMock = new Mock<ITablesFactory>();

            var service = new TablesService(repositoryMock.Object, unitOfWorkMock.Object, factoryMock.Object);
            var restaurantIdGuid = new Guid(restaurantId);
            var table = new Table()
            {
                RestaurantId = restaurantIdGuid,
                NumberOfPeople = numberOfPeople,
                NumberOfTables = numberOfTables
            };
            factoryMock.Setup(x => x.Create(restaurantIdGuid, numberOfPeople, numberOfTables)).Returns(table);
            var result = service.CreateTableType(restaurantIdGuid, numberOfPeople, numberOfTables);

            Assert.AreSame(table, result);
        }

        [TestCase("d547a40d-c45f-4c43-99de-0bfe9199ff95", 2)]
        [TestCase("d547a40d-c45f-4c43-99de-0bfe9199ff95", 4)]
        [TestCase("d547a40d-c45f-4c43-99de-0bfe9199ff95", 6)]
        public void MethodGetByRestaurantAndPeopleCountShould_CallRepositoryPropertyAll(string restaurantId, int peopleCount)
        {
            var repositoryMock = new Mock<IRepository<Table>>();
            var unitOfWorkMock = new Mock<IUnitOfWork>();
            var factoryMock = new Mock<ITablesFactory>();

            var service = new TablesService(repositoryMock.Object, unitOfWorkMock.Object, factoryMock.Object);
            var restaurantIdGuid = new Guid(restaurantId);

            service.GetByRestaurantAndPeopleCount(restaurantIdGuid, peopleCount);
            repositoryMock.Verify(r => r.All, Times.Once);
        }

        [TestCase("d547a40d-c45f-4c43-99de-0bfe9199ff95", 2)]
        [TestCase("d547a40d-c45f-4c43-99de-0bfe9199ff95", 4)]
        [TestCase("d547a40d-c45f-4c43-99de-0bfe9199ff95", 6)]
        public void MethodGetByRestaurantAndPeopleCountShould_ReturnNull_WhenThereAreNotSuchTables(string restaurantId, int peopleCount)
        {
            var repositoryMock = new Mock<IRepository<Table>>();
            var unitOfWorkMock = new Mock<IUnitOfWork>();
            var factoryMock = new Mock<ITablesFactory>();

            var service = new TablesService(repositoryMock.Object, unitOfWorkMock.Object, factoryMock.Object);
            var restaurantIdGuid = new Guid(restaurantId);
            var listAll = new List<Table>();

            repositoryMock.Setup(r => r.All).Returns(listAll.AsQueryable());

            var result = service.GetByRestaurantAndPeopleCount(restaurantIdGuid, peopleCount);
            Assert.IsNull(result);
        }

        [TestCase("d547a40d-c45f-4c43-99de-0bfe9199ff95", 2)]
        [TestCase("d547a40d-c45f-4c43-99de-0bfe9199ff95", 4)]
        [TestCase("d547a40d-c45f-4c43-99de-0bfe9199ff95", 6)]
        public void MethodGetByRestaurantAndPeopleCountShould_ReturnTables_WhenThereAreSuchTables(string restaurantId, int peopleCount)
        {
            var repositoryMock = new Mock<IRepository<Table>>();
            var unitOfWorkMock = new Mock<IUnitOfWork>();
            var factoryMock = new Mock<ITablesFactory>();

            var service = new TablesService(repositoryMock.Object, unitOfWorkMock.Object, factoryMock.Object);
            var restaurantIdGuid = new Guid(restaurantId);
            var table = new Table() { RestaurantId = restaurantIdGuid, NumberOfPeople = peopleCount };
            var listAll = new List<Table>() { table };

            repositoryMock.Setup(r => r.All).Returns(listAll.AsQueryable());

            var result = service.GetByRestaurantAndPeopleCount(restaurantIdGuid, peopleCount);
            Assert.AreSame(table, result);
        }

        [TestCase("d547a40d-c45f-4c43-99de-0bfe9199ff95", 2)]
        [TestCase("d547a40d-c45f-4c43-99de-0bfe9199ff95", 4)]
        [TestCase("d547a40d-c45f-4c43-99de-0bfe9199ff95", 6)]
        public void MethodGetTablesCountShould_CallRepositoryPropertyAll(string restaurantId, int peopleCount)
        {
            var repositoryMock = new Mock<IRepository<Table>>();
            var unitOfWorkMock = new Mock<IUnitOfWork>();
            var factoryMock = new Mock<ITablesFactory>();

            var service = new TablesService(repositoryMock.Object, unitOfWorkMock.Object, factoryMock.Object);
            var restaurantIdGuid = new Guid(restaurantId);

            service.GetTablesCount(restaurantIdGuid, peopleCount);
            repositoryMock.Verify(r => r.All, Times.Once);
        }

        [TestCase("d547a40d-c45f-4c43-99de-0bfe9199ff95", 2)]
        [TestCase("d547a40d-c45f-4c43-99de-0bfe9199ff95", 4)]
        [TestCase("d547a40d-c45f-4c43-99de-0bfe9199ff95", 6)]
        public void MethodGetTablesCountShould_ReturnZero_WhenThereIsNoTablesInThisPlace(string restaurantId, int peopleCount)
        {
            var repositoryMock = new Mock<IRepository<Table>>();
            var unitOfWorkMock = new Mock<IUnitOfWork>();
            var factoryMock = new Mock<ITablesFactory>();
            var listAll = new List<Table>();

            var service = new TablesService(repositoryMock.Object, unitOfWorkMock.Object, factoryMock.Object);
            var restaurantIdGuid = new Guid(restaurantId);
            repositoryMock
                .Setup(r => r.All)
                .Returns(listAll.AsQueryable<Table>());

            var result = service.GetTablesCount(restaurantIdGuid, peopleCount);

            Assert.AreEqual(0, result);
        }

        [TestCase("d547a40d-c45f-4c43-99de-0bfe9199ff95", 2)]
        [TestCase("d547a40d-c45f-4c43-99de-0bfe9199ff95", 4)]
        [TestCase("d547a40d-c45f-4c43-99de-0bfe9199ff95", 6)]
        public void MethodGetTablesCountShould_ReturnZero_WhenThereIsTableInThisPlace(string restaurantId, int peopleCount)
        {
            var repositoryMock = new Mock<IRepository<Table>>();
            var unitOfWorkMock = new Mock<IUnitOfWork>();
            var factoryMock = new Mock<ITablesFactory>();

            var service = new TablesService(repositoryMock.Object, unitOfWorkMock.Object, factoryMock.Object);
            var restaurantIdGuid = new Guid(restaurantId);
            var table = new Table()
            {
                RestaurantId = restaurantIdGuid,
                NumberOfPeople = peopleCount,
                NumberOfTables = 12
            };
            var listAll = new List<Table>() { table };

            repositoryMock
                .Setup(r => r.All)
                .Returns(listAll.AsQueryable<Table>());

            var result = service.GetTablesCount(restaurantIdGuid, peopleCount);

            Assert.AreEqual(12, result);
        }
    }
}
