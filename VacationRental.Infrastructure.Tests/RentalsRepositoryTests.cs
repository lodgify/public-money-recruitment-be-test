using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VacationalRental.Domain.Entities;
using VacationalRental.Infrastructure.DbContexts;
using VacationalRental.Infrastructure.Repositories;
using Xunit;

namespace VacationRental.Infrastructure.Tests
{
    public class RentalsRepositoryTests
    {
        public RentalsRepositoryTests()
        {
            InitContext();
        }

        private VacationRentalDbContext _vacationalRentalDbContext;

        private IEnumerable<RentalEntity> RentalEntitiesArrange { get; set; }

        private static bool HasBeenCreated { get; set; }
        internal void InitContext()
        {
            var builder = new DbContextOptionsBuilder<VacationRentalDbContext>().UseInMemoryDatabase(databaseName: "VacationalRentalAPI");
            var context = new VacationRentalDbContext(builder.Options);

            if (HasBeenCreated)
            {
                _vacationalRentalDbContext = context;
                return;
            }
                
            RentalEntitiesArrange = Enumerable.Range(1, 10)
                .Select(i => new RentalEntity { Id = i, PreprationTimeInDays = 2, Units = 5 });

            context.RentalEntities.AddRange(RentalEntitiesArrange);

            int changed = context.SaveChanges();

            _vacationalRentalDbContext = context;

            HasBeenCreated = true;
        }

        [Fact]
        public async Task GetRentalUnits()
        {
            var rentalsRepository = new RentalsRepository(_vacationalRentalDbContext);

            var result = await rentalsRepository.GetRentalUnits(rentalID: 2);

            Assert.Equal(5, result);
        }

        [Fact]
        public async Task GetRentalPreparationTimeInDays()
        {
            var rentalsRepository = new RentalsRepository(_vacationalRentalDbContext);

            var result = await rentalsRepository.GetRentalPreparationTimeInDays(rentalID: 1);

            Assert.Equal(2, result);
        }

        [Fact]
        public async Task InsertNewRentalObtainRentalId()
        {
            var rentalsRepository = new RentalsRepository(_vacationalRentalDbContext);

            var rentalEntity = new RentalEntity { PreprationTimeInDays = 1, Units = 1 };
            var result = await rentalsRepository.InsertNewRentalObtainRentalId(rentalEntity);

            Assert.Equal(11, result);
        }

        [Fact]
        public async Task GetRentalById()
        {
            var rentalsRepository = new RentalsRepository(_vacationalRentalDbContext);

            var result = await rentalsRepository.GetRentalById(1);

            Assert.Equal(1, result.Id);
            Assert.Equal(2, result.PreprationTimeInDays);
            Assert.Equal(5, result.Units);
        }

        [Fact]
        public async Task RentalExists()
        {
            var rentalsRepository = new RentalsRepository(_vacationalRentalDbContext);

            var result = await rentalsRepository.RentalExists(1);

            Assert.True(result);
        }

        [Fact]
        public async Task UpdateRental()
        {
            var rentalsRepository = new RentalsRepository(_vacationalRentalDbContext);

            var rentalEntity = new RentalEntity { Id = 8, PreprationTimeInDays = 1, Units = 1 };
            var result = await rentalsRepository.UpdateRental(rentalEntity);

            Assert.Equal(1, result);
        }
    }
}
