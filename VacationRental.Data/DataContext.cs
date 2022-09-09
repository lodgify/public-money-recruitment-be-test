using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;
using VacationRental.Data.Entities;

namespace VacationRental.Data
{
    public class DataContext: DbContext
    {
		public DataContext(DbContextOptions<DataContext> options)
			: base(options)
		{
		}

		public DataContext() : base()
		{

		}

		protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
		{
			base.OnConfiguring(optionsBuilder);
		}

		protected override void OnModelCreating(ModelBuilder modelBuilder)
		{
			base.OnModelCreating(modelBuilder);
		}

		public DbSet<Rental> Rentals { get; set; }
		public DbSet<Booking> Bookings { get; set; }
	}
}
