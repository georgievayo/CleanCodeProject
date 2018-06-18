using FindAndBook.Data;
using FindAndBook.Data.Contracts;
using FindAndBook.Tests.Data.Fake;
using Moq;
using NUnit.Framework;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;

namespace FindAndBook.Tests.Data
{
    [TestFixture]
    public class RepositoryTests
    {
        [Test]
        public void MathodAddShould_CallDbContextMethodSetAdded()
        {
            var dbContextMock = new Mock<IDbContext>();

            var repository = new Repository<FakeRepository>(dbContextMock.Object);

            var entity = new Mock<FakeRepository>();

            repository.Add(entity.Object);

            dbContextMock.Verify(c => c.SetAdded(entity.Object), Times.Once);
        }        

        [Test]
        public void AllShould_CallDbContextSet()
        {
            var data = this.GetData();

            var mockedSet = new Mock<IDbSet<FakeRepository>>();
            mockedSet.Setup(m => m.Provider).Returns(data.Provider);
            mockedSet.Setup(m => m.Expression).Returns(data.Expression);
            mockedSet.Setup(m => m.ElementType).Returns(data.ElementType);
            mockedSet.Setup(m => m.GetEnumerator()).Returns(data.GetEnumerator());

            var mockedDbContext = new Mock<IDbContext>();
            mockedDbContext.Setup(x => x.DbSet<FakeRepository>()).Returns(mockedSet.Object);

            var repository = new Repository<FakeRepository>(mockedDbContext.Object);

            var result = repository.All;

            mockedDbContext.Verify(db => db.DbSet<FakeRepository>(), Times.Once);
        }

        [Test]
        public void DeleteShould_CallDbContextSetDeleted()
        {
            var mockedDbContext = new Mock<IDbContext>();

            var repository = new Repository<FakeRepository>(mockedDbContext.Object);

            var entity = new Mock<FakeRepository>();

            repository.Delete(entity.Object);

            mockedDbContext.Verify(c => c.SetDeleted(entity.Object), Times.Once);
        }

        [TestCase(1)]
        [TestCase(432)]
        public void GetByIdShould_CallDbContextSetFind(int id)
        {
            var mockedSet = new Mock<DbSet<FakeRepository>>();

            var mockedDbContext = new Mock<IDbContext>();
            mockedDbContext.Setup(x => x.DbSet<FakeRepository>()).Returns(mockedSet.Object);

            var repository = new Repository<FakeRepository>(mockedDbContext.Object);

            repository.GetById(id);

            mockedSet.Verify(x => x.Find(id), Times.Once);
        }

        [TestCase(1)]
        [TestCase(432)]
        public void GetByIdShould_ReturnCorrectly(int id)
        {
            var mockedResult = new Mock<FakeRepository>();

            var mockedSet = new Mock<DbSet<FakeRepository>>();
            mockedSet.Setup(s => s.Find(It.IsAny<object>())).Returns(mockedResult.Object);

            var mockedDbContext = new Mock<IDbContext>();
            mockedDbContext.Setup(x => x.DbSet<FakeRepository>()).Returns(mockedSet.Object);

            var repository = new Repository<FakeRepository>(mockedDbContext.Object);

            var result = repository.GetById(id);

            Assert.AreSame(mockedResult.Object, result);
        }

        [Test]
        public void UpdateShould_CallDbContextSetUpdated()
        {
            var mockedDbContext = new Mock<IDbContext>();

            var repository = new Repository<FakeRepository>(mockedDbContext.Object);

            var entity = new Mock<FakeRepository>();

            repository.Update(entity.Object);

            mockedDbContext.Verify(c => c.SetUpdated(entity.Object), Times.Once);
        }

        private IQueryable<FakeRepository> GetData()
        {
            return new List<FakeRepository>
            {
                new FakeRepository(),
                new FakeRepository(),
                new FakeRepository()
            }.AsQueryable();
        }
    }
}
