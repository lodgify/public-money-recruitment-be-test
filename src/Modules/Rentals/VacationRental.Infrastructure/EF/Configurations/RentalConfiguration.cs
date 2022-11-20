using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using VacationRental.Core.Entities;

namespace VacationRental.Infrastructure.EF.Configurations
{
    internal class RentalConfiguration : IEntityTypeConfiguration<Rental>
    {
        public void Configure(EntityTypeBuilder<Rental> builder)
        {
            builder.Property(r => r.Id).ValueGeneratedOnAdd();

            builder
                .HasMany(r => r.Bookings)
                .WithOne(b => b.Rental);
        }
    }
}
