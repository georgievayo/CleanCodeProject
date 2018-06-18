using FindAndBook.Data.Contracts;
using System.Threading.Tasks;

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

        public async Task CommitAsync()
        {
            await this.dbContext.SaveChangesAsync();
        }
    }
}
