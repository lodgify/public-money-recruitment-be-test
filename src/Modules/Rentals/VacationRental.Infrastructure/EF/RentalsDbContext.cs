using Microsoft.EntityFrameworkCore;
using VacationRental.Core.Entities;

namespace VacationRental.Infrastructure.EF
{
    internal class RentalsDbContext : DbContext
    {
        public DbSet<Rental> Rentals { get; set; }
        public DbSet<Booking> Bookings { get; set; }

        public RentalsDbContext(DbContextOptions<RentalsDbContext> options) : base(options)
        {
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseInMemoryDatabase(databaseName: "RentalsDb");
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfigurationsFromAssembly(GetType().Assembly);
        }
    }
}
