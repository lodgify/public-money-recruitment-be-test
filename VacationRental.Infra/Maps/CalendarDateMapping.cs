using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using VacationRental.Domain.Calendars;

namespace VacationRental.Infra.Maps
{
	public class CalendarDateMapping : IEntityTypeConfiguration<CalendarDate>
	{
		public void Configure(EntityTypeBuilder<CalendarDate> builder)
		{
			builder.ToTable("CalendarDate");
			builder.HasKey(x => x.Id);
			builder.Property(x => x.StartDate);
			builder.Property(x => x.EndDate);
			builder.HasMany(x => x.Bookings).WithOne(x => x.CalendarDate);
		}
	}
}
