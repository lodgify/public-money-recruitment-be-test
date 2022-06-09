using Microsoft.EntityFrameworkCore;
using System.Reflection;
using VacationRental.DataAccess.Models.Entities;

namespace VacationRental.DataAccess.Contexts
{
    public class VacationRentalDbContext : DbContext
    {
        public VacationRentalDbContext(DbContextOptions<VacationRentalDbContext> options) : base(options)
        { }

        public DbSet<Booking>? Bookings { get; set; }
        public DbSet<Rental>? Rentals { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
        }
    }
}
