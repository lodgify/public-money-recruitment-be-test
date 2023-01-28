using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using VacationRental.Domain.PreparationTimes;

namespace VacationRental.Infra.Maps
{
	public class PreparationTimeMapping : IEntityTypeConfiguration<PreparationTime>
	{
		public void Configure(EntityTypeBuilder<PreparationTime> builder)
		{
			builder.ToTable("PreparationTime");
			builder.HasKey(x => x.Id);
			builder.Property(x => x.DateOfPreparation);
			builder.HasOne(x => x.Rental).WithMany(x => x.PreparationTimes).OnDelete(DeleteBehavior.Cascade);
		}
	}
}
