using Microsoft.EntityFrameworkCore;
using System.Reflection;
using VacationRental.DataAccess.Models.Entities;

namespace VacationRental.DataAccess
{
    public class VacationRentalDbContext : DbContext
    {
        public VacationRentalDbContext(DbContextOptions<VacationRentalDbContext> options) : base(options)
        {
            Database.SetCommandTimeout((int)TimeSpan.FromMinutes(10).TotalSeconds);
        }

        public DbSet<Booking>? Bookings { get; set; }
        public DbSet<Rental>? Rentals { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
        }
    }
}
