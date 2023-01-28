using Microsoft.EntityFrameworkCore;
using VacationRental.Domain.Bookings;
using VacationRental.Domain.Calendars;
using VacationRental.Domain.PreparationTimes;
using VacationRental.Domain.Rentals;

namespace VacationRental.Infra
{
	public class VacationRentalContext : DbContext
	{
		public VacationRentalContext(DbContextOptions<VacationRentalContext> options) : base(options)
		{

		}

		public DbSet<Booking> Bookings { get; set; }
		public DbSet<Rental> Rentals { get; set; }
		public DbSet<CalendarDate> CalendarDates { get; set; }
		public DbSet<PreparationTime> PreparationTimes { get; set; }
	}
}
