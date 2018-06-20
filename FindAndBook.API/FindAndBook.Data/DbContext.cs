using FindAndBook.Data.Contracts;
using FindAndBook.Models;
using System.ComponentModel.DataAnnotations.Schema;
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

        public DbSet<Booking> Bookings { get; set; }

        public DbSet<Restaurant> Restaurants { get; set; }

        public DbSet<Table> Tables { get; set; }

        public DbSet<BookedTables> BookedTables { get; set; }


        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Booking>()
                .Property(b => b.Id)
                .HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);
            modelBuilder.Entity<Booking>()
                .HasMany(x => x.Tables)
                .WithRequired(x => x.Booking)
                .WillCascadeOnDelete();

            modelBuilder.Entity<Restaurant>()
                .HasMany(x => x.Bookings)
                .WithRequired(x => x.Restaurant)
                .WillCascadeOnDelete();
            modelBuilder.Entity<Restaurant>()
                .HasMany(x => x.Tables)
                .WithRequired(x => x.Restaurant)
                .WillCascadeOnDelete();
            modelBuilder.Entity<Restaurant>()
                .HasRequired(x => x.Manager)
                .WithMany(x => x.Restaurants)
                .WillCascadeOnDelete();

            modelBuilder.Entity<Table>()
                .Property(x => x.Id)
                .HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);

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
