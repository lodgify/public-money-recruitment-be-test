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
		}
	}
}
