using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using VR.Domain.Models;

namespace VR.DataAccess.EntityConfigurations
{
    public class BookingEntityTypeConfiguration : IEntityTypeConfiguration<Booking>
    {
        public void Configure(EntityTypeBuilder<Booking> builder)
        {
            builder.ToTable("tblBooking");
            builder.HasKey(e => e.Id).HasName("PK_tblBooking_BookingId");
            builder.Property(e => e.Id).IsUnicode(false);
            builder.Property(e => e.RentalId).IsUnicode(false).IsRequired();
            builder.Property(e => e.Start).IsRequired();
            builder.Property(e => e.Nights).IsRequired();

            builder.HasOne(d => d.Rental)
               .WithMany(p => p.Bookings)
               .HasForeignKey(d => d.RentalId)
               .OnDelete(DeleteBehavior.ClientSetNull)
               .HasConstraintName("FK_tblAthlete_tblTeam");
        }
    }
}
