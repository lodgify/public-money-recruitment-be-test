using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using VacationRental.Domain.Bookings;

namespace VacationRental.Infra.Maps
{
	public class BookingMapping : IEntityTypeConfiguration<Booking>
	{
		public void Configure(EntityTypeBuilder<Booking> builder)
		{
			builder.ToTable("Booking");
			builder.HasKey(x => x.Id);
			builder.Property(x => x.Unity);
			builder.Property(x => x.Nights);
			builder.HasOne(x => x.Rental).WithMany(x => x.Bookings).HasForeignKey(w => w.RentalId).OnDelete(DeleteBehavior.Restrict);
		}
	}
}
