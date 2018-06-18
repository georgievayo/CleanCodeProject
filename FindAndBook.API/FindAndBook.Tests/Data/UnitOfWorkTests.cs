using FindAndBook.Data;
using FindAndBook.Data.Contracts;
using Moq;
using NUnit.Framework;

namespace FindAndBook.Tests.Data
{
    [TestFixture]
    public class UnitOfWorkTests
    {
        [Test]
        public void CommitShould_CallDbContextSaveChanges()
        {
            var mockedDbContext = new Mock<IDbContext>();

            var unitOfWork = new UnitOfWork(mockedDbContext.Object);

            unitOfWork.Commit();

            mockedDbContext.Verify(c => c.SaveChanges(), Times.Once);
        }

        [Test]
        public void CommitAsyncShould_CallDbContextSaveChangesAsync()
        {
            var mockedDbContext = new Mock<IDbContext>();

            var unitOfWork = new UnitOfWork(mockedDbContext.Object);

            unitOfWork.CommitAsync();

            mockedDbContext.Verify(c => c.SaveChangesAsync(), Times.Once);
        }
    }
}
