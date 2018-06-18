using FindAndBook.Data.Contracts;
using System.Linq;

namespace FindAndBook.Data
{
    public class Repository<T> : IRepository<T>
        where T : class
    {
        private readonly IDbContext dbContext;

        public Repository(IDbContext dbContext)
        {
            this.dbContext = dbContext;
        }

        public IQueryable<T> All
        {
            get
            {
                return this.dbContext.DbSet<T>();
            }
        }

        public void Add(T entity)
        {
            this.dbContext.SetAdded(entity);
        }

        public void Delete(T entity)
        {
            this.dbContext.SetDeleted(entity);
        }

        public T GetById(object id)
        {
            return this.dbContext.DbSet<T>().Find(id);
        }

        public void Update(T entity)
        {
            this.dbContext.SetUpdated(entity);
        }
    }
}
