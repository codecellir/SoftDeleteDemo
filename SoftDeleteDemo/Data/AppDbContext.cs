using Microsoft.EntityFrameworkCore;

namespace SoftDeleteDemo.Data
{
    public class AppDbContext(DbContextOptions<AppDbContext> options):DbContext(options)
    {
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Book>().HasQueryFilter(_ => !_.IsDeleted);

            modelBuilder.Entity<Book>()
                .HasIndex(_ => _.IsDeleted)
                .HasFilter("IsDeleted = 0");
        }

        public DbSet<Book> Books => Set<Book>();
    }
}
