using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using VacationRental.Domain;

namespace VacationRental.Migrations
{
    public class VacationRentalDesignTimeDbContextFactory : IDesignTimeDbContextFactory<VacationRentalDbContext>
    {
        public VacationRentalDbContext CreateDbContext(string[] args)
        {
            var optionsBuilder = new DbContextOptionsBuilder<VacationRentalDbContext>();
            optionsBuilder.UseSqlServer(
                "Server=.;Database=VacationRental;Trusted_Connection=True;",
                options =>
                {
                    options.MigrationsHistoryTable(VacationRentalDbContext.MigrationsTableName, VacationRentalDbContext.MigrationsSchemaName);
                    options.MigrationsAssembly(typeof(VacationRentalDesignTimeDbContextFactory).Assembly.ToString());
                });

            return new VacationRentalDbContext(optionsBuilder.Options);
        }
    }
}
