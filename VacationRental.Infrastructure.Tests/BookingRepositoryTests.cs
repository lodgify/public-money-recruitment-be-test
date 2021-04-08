using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using VacationalRental.Domain.Entities;
using VacationalRental.Infrastructure.DbContexts;
using VacationalRental.Infrastructure.Repositories;
using Xunit;

namespace VacationRental.Infrastructure.Tests
{
    public class BookingRepositoryTests
    {
        public BookingRepositoryTests()
        {
            InitContext();
        }

        private VacationRentalDbContext _vacationalRentalDbContext;

        private IEnumerable<BookingEntity> BookingEntitiesArrange { get; set; }

        internal void InitContext()
        {
            var builder = new DbContextOptionsBuilder<VacationRentalDbContext>().UseInMemoryDatabase(databaseName: "VacationalRentalAPI");

            var context = new VacationRentalDbContext(builder.Options);

            BookingEntitiesArrange = Enumerable.Range(1, 10)
                .Select(i => new BookingEntity { RentalId = 1, Nights = i, Start = DateTime.Now.Date, Unit = i });

            context.BookingEntities.AddRange(BookingEntitiesArrange);
            int changed = context.SaveChanges();
            _vacationalRentalDbContext = context;
        }

        [Fact]
        public async Task GetBookinByRentalId()
        {
            var bookingRepository = new BookingsRepository(_vacationalRentalDbContext);

            var bookingEntitiesArr = BookingEntitiesArrange.ToList();

            for (int i = 0; i < bookingEntitiesArr.Count(); i++)
            {
                int bookingId = i + 1;
                bookingEntitiesArr[i].Id = bookingId;
            }

            var result = await bookingRepository.GetBookinByRentalId(rentalId: 1);

            var expected = result.ToList();

            for (int i = 0; i < expected.Count(); i++)
            {
                Assert.Equal(expected[i].Id, bookingEntitiesArr[i].Id);
                Assert.Equal(expected[i].Nights, bookingEntitiesArr[i].Nights);
                Assert.Equal(expected[i].RentalId, bookingEntitiesArr[i].RentalId);
                Assert.Equal(expected[i].Start, bookingEntitiesArr[i].Start);
                Assert.Equal(expected[i].Unit, bookingEntitiesArr[i].Unit);
            }
        }

        [Fact]
        public async Task GetBookingById()
        {
            var bookingRepository = new BookingsRepository(_vacationalRentalDbContext);

            var bookingEntitiesArr = BookingEntitiesArrange.ToList();

            for (int i = 0; i < bookingEntitiesArr.Count(); i++)
            {
                int bookingId = i + 1;
                bookingEntitiesArr[i].Id = bookingId;
            }

            var expected = await bookingRepository.GetBookingById(bookingId: 1);

            var actual = bookingEntitiesArr.Where(a => a.Id == 1).Single();

            Assert.Equal(expected.Id, actual.Id);
            Assert.Equal(expected.Nights, actual.Nights);
            Assert.Equal(expected.RentalId, actual.RentalId);
            Assert.Equal(expected.Start, actual.Start);
            Assert.Equal(expected.Unit, actual.Unit);
        }

        [Fact]
        public async Task BookingExists()
        {
            var bookingRepository = new BookingsRepository(_vacationalRentalDbContext);

            var result = await bookingRepository.BookingExists(1);

            Assert.True(result);
        }

        [Fact]
        public async Task GetLastUnit()
        {
            var bookingRepository = new BookingsRepository(_vacationalRentalDbContext);

            var result = await bookingRepository.GetLastUnit(1);

            Assert.Equal(10, result);
        }
    }
}
