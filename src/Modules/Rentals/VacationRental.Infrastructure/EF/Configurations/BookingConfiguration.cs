using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using VacationRental.Core.Entities;

namespace VacationRental.Infrastructure.EF.Configurations
{
    internal class BookingConfiguration : IEntityTypeConfiguration<Rental>
    {
        public void Configure(EntityTypeBuilder<Rental> builder)
        {
            builder
                .HasMany(r => r.Bookings)
                .WithOne(b => b.Rental);
        }
    }
}
