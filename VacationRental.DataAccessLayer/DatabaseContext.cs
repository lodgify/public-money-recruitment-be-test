using VacationRental.SqlDataAccess.Configs;
using Microsoft.EntityFrameworkCore;

namespace VacationRental.SqlDataAccess
{
	public class DatabaseContext : DbContext
	{
		public DatabaseContext() : base() { }

		public DatabaseContext(DbContextOptions<DatabaseContext> options) : base(options)
		{
		}
		protected override void OnModelCreating(ModelBuilder modelBuilder)
		{
			base.OnModelCreating(modelBuilder);

			modelBuilder.ApplyConfiguration(new BookingConfig());
			modelBuilder.ApplyConfiguration(new RentalConfig());
		}
	}
}
