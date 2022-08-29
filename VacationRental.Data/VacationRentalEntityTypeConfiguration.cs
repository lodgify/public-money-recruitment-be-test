using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using VacationRental.Core;
using VacationRental.Core.Domain;

namespace VacationRental.Data
{
    public class VacationRentalEntityTypeConfiguration<TEntity, TId> : IEntityTypeConfiguration<TEntity> where TEntity : BaseEntity<TId>
    {
        public virtual void Configure(EntityTypeBuilder<TEntity> builder)
        {
            // Do nothing
        }
    }
}
