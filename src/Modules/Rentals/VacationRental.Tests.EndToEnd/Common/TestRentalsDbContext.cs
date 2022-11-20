using Microsoft.EntityFrameworkCore;
using VacationRental.Infrastructure.EF;
using VacationRental.Shared.Tests;

namespace VacationRental.Tests.EndToEnd.Common
{
    internal class TestRentalsDbContext : TestDbContext<RentalsDbContext>
    {
        private static DbContextOptions<RentalsDbContext> _dbContextOptions = new DbContextOptionsBuilder<RentalsDbContext>()
            .UseInMemoryDatabase(databaseName: "RentalsDbEndToEndTests")
            .Options;

        protected override RentalsDbContext Init(string connectionString)
            => new(_dbContextOptions);
    }
}
