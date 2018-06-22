using FindAndBook.Data.Contracts;
using FindAndBook.Models;
using System.Data.Entity;

namespace FindAndBook.Data
{
    public partial class DbContext : System.Data.Entity.DbContext, IDbContext
    {
        public DbContext() : base("FindAndBook")
        {
            Database.SetInitializer<DbContext>(null);
        }

        public static DbContext Create()
        {
            return new DbContext();
        }

        public DbSet<User> Users { get; set; }

        public DbSet<Manager> Managers { get; set; }

        public DbSet<Booking> Bookings { get; set; }

        public DbSet<Restaurant> Restaurants { get; set; }


        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Restaurant>()
                .HasMany(x => x.Bookings)
                .WithRequired(x => x.Restaurant)
                .WillCascadeOnDelete();

            modelBuilder.Entity<Restaurant>()
                .HasRequired(x => x.Manager)
                .WithMany(x => x.Restaurants)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<User>()
                .HasMany(u => u.Bookings)
                .WithRequired(b => b.User)
                .WillCascadeOnDelete();

            base.OnModelCreating(modelBuilder);
        }

        public IDbSet<TEntity> DbSet<TEntity>() where TEntity : class
        {
            return this.Set<TEntity>();
        }

        public void SetAdded<TEntry>(TEntry entity) where TEntry : class
        {
            var entry = this.Entry(entity);
            entry.State = EntityState.Added;
        }

        public void SetDeleted<TEntry>(TEntry entity) where TEntry : class
        {
            var entry = this.Entry(entity);
            entry.State = EntityState.Deleted;
        }

        public void SetUpdated<TEntry>(TEntry entity) where TEntry : class
        {
            var entry = this.Entry(entity);
            entry.State = EntityState.Modified;
        }
    }
}
