using Microsoft.EntityFrameworkCore;
using VacationRental.DataAccess.Contexts;

namespace VacationRental.Services.UnitTests.Common
{
    public class VacationRentalSeedDataFixture : IDisposable
    {
        public VacationRentalDbContext VacationRentalDbContext { get; private set; }

        public VacationRentalSeedDataFixture()
        {
            const string databaseName = "VacationRentalDb";

            var options = new DbContextOptionsBuilder<VacationRentalDbContext>().UseInMemoryDatabase(databaseName).Options;

            VacationRentalDbContext = new VacationRentalDbContext(options);
        }

        public void Dispose()
        {
            VacationRentalDbContext.Dispose();
        }
    }
}
