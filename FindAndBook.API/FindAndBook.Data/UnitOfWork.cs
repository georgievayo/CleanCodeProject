using FindAndBook.Data.Contracts;

namespace FindAndBook.Data
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly IDbContext dbContext;

        public UnitOfWork(IDbContext dbContext)
        {
            this.dbContext = dbContext;
        }

        public void Commit()
        {
            this.dbContext.SaveChanges();
        }
    }
}
