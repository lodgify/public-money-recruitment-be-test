using Microsoft.EntityFrameworkCore;
using VacationRental.Data.Mapping;

namespace VacationRental.Data
{
    public class VacationRentalObjectContext : DbContext
    {
        public VacationRentalObjectContext(DbContextOptions<VacationRentalObjectContext> options) 
            : base(options)
        {
            Init();
        }

        private void Init()
        {
            Database.EnsureCreated();
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfiguration(new BookingEntityMap());
            modelBuilder.ApplyConfiguration(new RentalEntityMap());

            base.OnModelCreating(modelBuilder);
        }
    }
}
