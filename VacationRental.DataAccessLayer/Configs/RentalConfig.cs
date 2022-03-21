using VacationRental.Domain.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace VacationRental.SqlDataAccess.Configs
{
	public class RentalConfig : IEntityTypeConfiguration<Rental>
	{
		public void Configure(EntityTypeBuilder<Rental> builder)
		{
			builder.ToTable("rental");

			builder.Property(f => f.Id)
				.ValueGeneratedOnAdd();

			builder.Property(f => f.Units);
			
			builder.HasKey(f => f.Id);
		}
	}
}
