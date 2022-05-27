using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;
using VacationRental.Domain;
using VacationRental.Domain.Entities;

namespace VacationRental.Persistance
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {

        }

        public DbSet<RentalEntity> RentalEntity { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
        
            builder.Entity<RentalEntity>(cfg =>
            {
                cfg.HasKey(e => e.Id);

                cfg.Property(e => e.Id)
                    .IsRequired();
                cfg.Property(e => e.Units)
                    .IsRequired();
                cfg.Property(e => e.PreparationTimeInDays)
                    .IsRequired();
            });

            builder.Entity<BookingEntity>(cfg =>
            {
                cfg.HasKey(e => e.Id);
                cfg.Property(e => e.Id)
                    .IsRequired();
                cfg.Property(e => e.Nights)
                    .IsRequired();
                cfg.Property(e => e.RentalId)
                    .IsRequired();
                cfg.Property(e => e.Start)
                   .IsRequired();
                   
            });
        }
    }
}
