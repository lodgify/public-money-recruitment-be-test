using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using VacationRental.Core.Domain.Bookings;

namespace VacationRental.Data.Mapping
{
    public class BookingEntityMap : VacationRentalEntityTypeConfiguration<BookingEntity, int>
    {
        public override void Configure(EntityTypeBuilder<BookingEntity> builder)
        {
            builder.ToTable("Bookings", "dbo");

            builder.HasKey(x => x.Id);
            builder.Property(x => x.Id)
                .ValueGeneratedOnAdd();

            builder.HasOne(x => x.Rental)
                .WithMany(x => x.Bookings)
                .HasForeignKey(m => m.RentalId)
                .IsRequired()
                .OnDelete(DeleteBehavior.Cascade);

            base.Configure(builder);
        }
    }
}
