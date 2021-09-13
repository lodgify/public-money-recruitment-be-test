using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using VacationRental.Business;

namespace VacationRental.Data
{
    public class Context : Microsoft.EntityFrameworkCore.DbContext
    {
        private IConfiguration _configuration;

        public Context(IConfiguration configuration) : base()
        {
            _configuration = configuration;
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer(_configuration.GetConnectionString("EICAConnectionString"));

            base.OnConfiguring(optionsBuilder);
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {

            modelBuilder.Entity<Booking>(config =>
            {
                config.ToTable("Booking");
                config.HasKey(k => new { k.Id });
                config.Property(p => p.NumberOfNights).HasColumnName("NumberOfNights");
                /* ... */
            });
        }
    
    }
}
