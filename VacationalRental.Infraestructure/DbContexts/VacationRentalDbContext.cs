using Microsoft.EntityFrameworkCore;
using VacationalRental.Domain.Entities;

namespace VacationalRental.Infrastructure.DbContexts
{
    public class VacationRentalDbContext : DbContext
    {
        public VacationRentalDbContext(DbContextOptions<VacationRentalDbContext> options) : base(options)
        {

        }

        public DbSet<BookingEntity> BookingEntities { get; set; }
        public DbSet<RentalEntity> RentalEntities { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<BookingEntity>()
                .Property(c => c.RowVersion)
                .IsRowVersion();

            modelBuilder.Entity<BookingEntity>()
                .HasKey(a => a.Id);

            modelBuilder.Entity<RentalEntity>()
                .Property(c => c.RowVersion)
                .IsRowVersion();

            modelBuilder.Entity<RentalEntity>()
                .HasKey(a => a.Id);
        }
    }
}
