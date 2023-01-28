using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using VacationRental.Domain.Rentals;

namespace VacationRental.Infra.Maps
{
	public class RentalMapping : IEntityTypeConfiguration<Rental>
	{
		public void Configure(EntityTypeBuilder<Rental> builder)
		{
			builder.ToTable("Rental");
			builder.HasKey(x => x.Id);
			builder.Property(x => x.Units);
			builder.HasMany(x => x.Bookings).WithOne(x => x.Rental).OnDelete(DeleteBehavior.Cascade); 
			builder.HasMany(x => x.PreparationTimes).WithOne(x => x.Rental).OnDelete(DeleteBehavior.Cascade);
		}
	}
}
