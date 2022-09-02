using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using VR.Domain.Models;

namespace VR.DataAccess.EntityConfigurations
{
    public class RentalEntityTypeConfigurationn : IEntityTypeConfiguration<Rental>
    {
        public void Configure(EntityTypeBuilder<Rental> builder)
        {
            builder.ToTable("tblRental");
            builder.HasKey(e => e.Id).HasName("PK_tblRental_RentalId");
            builder.Property(e => e.Id).IsUnicode(false);
            builder.Property(e => e.PreparationTimeInDays).IsUnicode(false).IsRequired();
            builder.Property(e => e.Units).IsUnicode(false).IsRequired();
        }
    }
}
