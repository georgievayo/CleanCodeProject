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
    public class BookedTablesServiceTests
    {
        [TestCase(10)]
        [TestCase(2)]
        [TestCase(4)]
        public void MethodBookTableShould_CallFactoryMethodCreate(int tablesCount)
        {
            var repositoryMock = new Mock<IRepository<BookedTables>>();
            var unitOfWorkMock = new Mock<IUnitOfWork>();
            var factoryMock = new Mock<IBookedTablesFactory>();

            var service = new BookedTablesService(repositoryMock.Object,
                unitOfWorkMock.Object, factoryMock.Object);

            var bookingId = Guid.NewGuid();
            var tableId = Guid.NewGuid();

            service.BookTable(bookingId, tableId, tablesCount);

            factoryMock.Verify(f => f.CreateBookedTable(bookingId, tableId, tablesCount));
        }

        [TestCase(10)]
        [TestCase(2)]
        [TestCase(4)]
        public void MethodBookTableShould_CallRepositoryMethodAdd(int tablesCount)
        {
            var repositoryMock = new Mock<IRepository<BookedTables>>();
            var unitOfWorkMock = new Mock<IUnitOfWork>();
            var factoryMock = new Mock<IBookedTablesFactory>();

            var service = new BookedTablesService(repositoryMock.Object,
                unitOfWorkMock.Object, factoryMock.Object);

            var bookingId = Guid.NewGuid();
            var tableId = Guid.NewGuid();
            var bookedTable = new BookedTables()
            {
                BookingId = bookingId,
                TableId = tableId,
                TablesCount = tablesCount
            };
            factoryMock.Setup(f => f.CreateBookedTable(bookingId, tableId, tablesCount)).Returns(bookedTable);
            service.BookTable(bookingId, tableId, tablesCount);

            repositoryMock.Verify(r => r.Add(bookedTable), Times.Once);
        }

        [TestCase(10)]
        [TestCase(2)]
        [TestCase(4)]
        public void MethodBookTableShould_CallUnitOfWorkMethodCommit(int tablesCount)
        {
            var repositoryMock = new Mock<IRepository<BookedTables>>();
            var unitOfWorkMock = new Mock<IUnitOfWork>();
            var factoryMock = new Mock<IBookedTablesFactory>();

            var service = new BookedTablesService(repositoryMock.Object,
                unitOfWorkMock.Object, factoryMock.Object);

            var bookingId = Guid.NewGuid();
            var tableId = Guid.NewGuid();
            var bookedTable = new BookedTables()
            {
                BookingId = bookingId,
                TableId = tableId,
                TablesCount = tablesCount
            };
            factoryMock.Setup(f => f.CreateBookedTable(bookingId, tableId, tablesCount)).Returns(bookedTable);
            service.BookTable(bookingId, tableId, tablesCount);

            unitOfWorkMock.Verify(u => u.Commit(), Times.Once);
        }

        [Test]
        public void MethodGetAllByBookingShould_CallRepositoryMethodAll()
        {
            var repositoryMock = new Mock<IRepository<BookedTables>>();
            var unitOfWorkMock = new Mock<IUnitOfWork>();
            var factoryMock = new Mock<IBookedTablesFactory>();

            var service = new BookedTablesService(repositoryMock.Object,
                unitOfWorkMock.Object, factoryMock.Object);
            var bookindId = Guid.NewGuid();

            service.GetAllByBooking(bookindId);

            repositoryMock.Verify(r => r.All, Times.Once);
        }

        [Test]
        public void MethodGetAllByBookingShould_ReturnCorrectResult()
        {
            var repositoryMock = new Mock<IRepository<BookedTables>>();
            var unitOfWorkMock = new Mock<IUnitOfWork>();
            var factoryMock = new Mock<IBookedTablesFactory>();

            var bookindId = Guid.NewGuid();
            var bookedTable = new BookedTables() { BookingId = bookindId };
            var list = new List<BookedTables>() { bookedTable };
            repositoryMock.Setup(r => r.All).Returns(list.AsQueryable());

            var service = new BookedTablesService(repositoryMock.Object,
                unitOfWorkMock.Object, factoryMock.Object);

            var result = service.GetAllByBooking(bookindId);

            Assert.AreSame(bookedTable, result);
        }

        [TestCase("d547a40d-c45f-4c43-99de-0bfe9199ff95")]
        public void MethodDeleteAllByBookingShould_CallRepositoryPropertyAll(string bookingId)
        {
            var repositoryMock = new Mock<IRepository<BookedTables>>();
            var unitOfWorkMock = new Mock<IUnitOfWork>();
            var factoryMock = new Mock<IBookedTablesFactory>();

            var bookingIdGuid = new Guid(bookingId);
            var service = new BookedTablesService(repositoryMock.Object, unitOfWorkMock.Object, factoryMock.Object);
            service.DeleteAllByBooking(bookingIdGuid);

            repositoryMock.Verify(r => r.All, Times.Once);
        }

        [TestCase("d547a40d-c45f-4c43-99de-0bfe9199ff95")]
        public void MethodDeleteAllByBookingShould_CallRepositoryMethodDeleteForEachBooking(string bookingId)
        {
            var repositoryMock = new Mock<IRepository<BookedTables>>();
            var unitOfWorkMock = new Mock<IUnitOfWork>();
            var factoryMock = new Mock<IBookedTablesFactory>();

            var bookingIdGuid = new Guid(bookingId);
            var bookedTable = new BookedTables() { BookingId = bookingIdGuid };
            var list = new List<BookedTables>() { bookedTable };
            repositoryMock.Setup(r => r.All).Returns(list.AsQueryable());

            var service = new BookedTablesService(repositoryMock.Object, unitOfWorkMock.Object, factoryMock.Object);
            service.DeleteAllByBooking(bookingIdGuid);

            repositoryMock.Verify(r => r.Delete(bookedTable), Times.Once);
        }

        [TestCase("d547a40d-c45f-4c43-99de-0bfe9199ff95")]
        public void MethodDeleteAllByBookingShould_CallUnitOfWorkMethodCommitForEachBooking(string bookingId)
        {
            var repositoryMock = new Mock<IRepository<BookedTables>>();
            var unitOfWorkMock = new Mock<IUnitOfWork>();
            var factoryMock = new Mock<IBookedTablesFactory>();

            var bookingIdGuid = new Guid(bookingId);
            var bookedTable = new BookedTables() { BookingId = bookingIdGuid };
            var list = new List<BookedTables>() { bookedTable };
            repositoryMock.Setup(r => r.All).Returns(list.AsQueryable());

            var service = new BookedTablesService(repositoryMock.Object, unitOfWorkMock.Object, factoryMock.Object);
            service.DeleteAllByBooking(bookingIdGuid);

            unitOfWorkMock.Verify(r => r.Commit(), Times.Once);
        }
    }
}
