using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using VacationRental.Core.Domain.Rentals;

namespace VacationRental.Data.Mapping
{
    public class RentalEntityMap : VacationRentalEntityTypeConfiguration<RentalEntity, int>
    {
        public override void Configure(EntityTypeBuilder<RentalEntity> builder)
        {
            builder.ToTable("Rentals", "dbo");

            builder.HasKey(x => x.Id);
            builder.Property(x => x.Id)
                .ValueGeneratedOnAdd();

            base.Configure(builder);
        }
    }
}
