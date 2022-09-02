using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System;
using VR.DataAccess.EntityConfigurations;
using VR.Domain.Models;

namespace VR.DataAccess
{
    public class VRContext : DbContext
    {
        public VRContext(DbContextOptions options) : base(options) { }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.HasAnnotation("Relational:Collation", "SQL_Latin1_General_CP1_CI_AS");

            new BookingEntityTypeConfiguration().Configure(modelBuilder.Entity<Booking>());
            new RentalEntityTypeConfigurationn().Configure(modelBuilder.Entity<Rental>());
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                var configuration = new ConfigurationBuilder()
                    .SetBasePath(AppContext.BaseDirectory)
                    .AddJsonFile("appsettings.json")
                    .Build();

                BuildOptions(optionsBuilder, configuration);
            }
        }

        protected static DbContextOptionsBuilder BuildOptions(
            DbContextOptionsBuilder optionsBuilder,
            IConfiguration configuration)
        {
            return optionsBuilder.UseSqlServer(configuration["ConnectionString"],
                options => options.EnableRetryOnFailure(maxRetryCount: 5));
        }

        public static DbContextOptionsBuilder<TContext> BuildOptions<TContext>(
            DbContextOptionsBuilder<TContext> optionsBuilder,
            IConfiguration configuration) where TContext : DbContext
        {
            return BuildOptions(optionsBuilder, configuration);
        }

        public virtual DbSet<Booking> Bookings { get; set; }
        public virtual DbSet<Rental> Rentals { get; set; }
    }
}
