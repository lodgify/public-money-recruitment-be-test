using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using VacationRental.DataAccess.Models.Entities;

namespace VacationRental.DataAccess.Configurations
{
    public class BookingConfiguration : IEntityTypeConfiguration<Booking>
    {
        public void Configure(EntityTypeBuilder<Booking> builder)
        {
            builder.HasKey(entity => entity.Id);

            builder.Property(entity => entity.Start);
            builder.Property(entity => entity.RentalId);
            builder.Property(entity => entity.Nights);

            builder.Property(entity => entity.Created);
            builder.Property(entity => entity.Modified);
            builder.Property(entity => entity.IsActive);
        }
    }
}
