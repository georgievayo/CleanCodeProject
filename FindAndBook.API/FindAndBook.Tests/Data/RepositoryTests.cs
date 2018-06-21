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
        private Mock<IDbContext> dbContextMock;
        private Mock<FakeEntity> entity;

        [Test]
        public void MathodAddShould_CallDbContextMethodSetAdded()
        {
            var repository = new Repository<FakeEntity>(dbContextMock.Object);

            repository.Add(entity.Object);

            dbContextMock.Verify(c => c.SetAdded(entity.Object), Times.Once);
        }        

        [Test]
        public void AllShould_CallDbContextSet()
        {
            var data = this.GetData();

            var mockedSet = new Mock<IDbSet<FakeEntity>>();
            mockedSet.Setup(m => m.Provider).Returns(data.Provider);
            mockedSet.Setup(m => m.Expression).Returns(data.Expression);
            mockedSet.Setup(m => m.ElementType).Returns(data.ElementType);
            mockedSet.Setup(m => m.GetEnumerator()).Returns(data.GetEnumerator());

            dbContextMock.Setup(x => x.DbSet<FakeEntity>()).Returns(mockedSet.Object);

            var repository = new Repository<FakeEntity>(dbContextMock.Object);

            var result = repository.All;

            dbContextMock.Verify(db => db.DbSet<FakeEntity>(), Times.Once);
        }

        [Test]
        public void DeleteShould_CallDbContextSetDeleted()
        {
            var repository = new Repository<FakeEntity>(dbContextMock.Object);

            repository.Delete(entity.Object);

            dbContextMock.Verify(c => c.SetDeleted(entity.Object), Times.Once);
        }

        [TestCase(1)]
        [TestCase(432)]
        public void GetByIdShould_CallDbContextSetFind(int id)
        {
            var mockedSet = new Mock<DbSet<FakeEntity>>();
            dbContextMock.Setup(x => x.DbSet<FakeEntity>()).Returns(mockedSet.Object);

            var repository = new Repository<FakeEntity>(dbContextMock.Object);

            repository.GetById(id);

            mockedSet.Verify(x => x.Find(id), Times.Once);
        }

        [TestCase(1)]
        [TestCase(432)]
        public void GetByIdShould_ReturnCorrectly(int id)
        {
            var mockedSet = new Mock<DbSet<FakeEntity>>();
            mockedSet.Setup(s => s.Find(It.IsAny<object>())).Returns(entity.Object);
            dbContextMock.Setup(x => x.DbSet<FakeEntity>()).Returns(mockedSet.Object);

            var repository = new Repository<FakeEntity>(dbContextMock.Object);

            var result = repository.GetById(id);

            Assert.AreSame(entity.Object, result);
        }

        [Test]
        public void UpdateShould_CallDbContextSetUpdated()
        {
            var repository = new Repository<FakeEntity>(dbContextMock.Object);

            repository.Update(entity.Object);

            dbContextMock.Verify(c => c.SetUpdated(entity.Object), Times.Once);
        }

        private IQueryable<FakeEntity> GetData()
        {
            return new List<FakeEntity>
            {
                new FakeEntity(),
                new FakeEntity(),
                new FakeEntity()
            }.AsQueryable();
        }

        [SetUp]
        public void SetUp()
        {
            dbContextMock = new Mock<IDbContext>();
            entity = new Mock<FakeEntity>();
        }
    }
}
