using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using VacationRental.Domain.Entities;

namespace VacationRental.Domain.Configurations
{
    public class BookingConfiguration : IEntityTypeConfiguration<Booking>
    {
        public void Configure(EntityTypeBuilder<Booking> builder)
        {
            builder.HasKey(x => x.Id);

            builder.HasOne(x => x.Unit)
                   .WithMany(x => x.Bookings)
                   .HasForeignKey(p => p.UnitId);
        }
    }
}
