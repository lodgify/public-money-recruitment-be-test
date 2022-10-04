using Microsoft.EntityFrameworkCore;
using VacationRental.Domain.Entities;

namespace VacationRental.Domain
{
    public class VacationRentalDbContext : DbContext
    {
        public const string MigrationsSchemaName = "_migrations";
        public const string MigrationsTableName = "Migrations";
        
        public VacationRentalDbContext(DbContextOptions<VacationRentalDbContext> options)
            : base(options)
        {
        }

        public DbSet<Booking> Bookings { get; set; }

        public DbSet<Rental> Rentals { get; set; }

        public DbSet<Unit> Units { get; set; }
    }
}