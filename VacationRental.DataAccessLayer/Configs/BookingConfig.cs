using VacationRental.Domain.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace VacationRental.SqlDataAccess.Configs
{
	/// Entity Framework map the table design to the Booking model, all Booking table schema specifications should be defined here.
	public class BookingConfig : IEntityTypeConfiguration<Booking>
	{
		public void Configure(EntityTypeBuilder<Booking> builder)
		{
			builder.ToTable("booking");

			builder.Property(f => f.Id)
				.ValueGeneratedOnAdd();
			builder.Property(f => f.Nights);
			builder.Property(f => f.RentalId);
			builder.Property(f => f.Start);
			builder.Property(f => f.IsPreparationTime)
				.HasDefaultValue(false);

			builder.HasKey(f => f.Id);
		}
	}
}
