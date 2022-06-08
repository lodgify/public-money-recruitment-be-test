using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using VacationRental.DataAccess.Models.Entities;

namespace VacationRental.DataAccess.Configurations
{
    public class RentalConfiguration : IEntityTypeConfiguration<Rental>
    {
        public void Configure(EntityTypeBuilder<Rental> builder)
        {
            builder.HasKey(entity => entity.Id);

            builder.Property(entity => entity.Units);

            builder.Property(entity => entity.Created);
            builder.Property(entity => entity.Modified);
            builder.Property(entity => entity.IsActive);
        }
    }
}
