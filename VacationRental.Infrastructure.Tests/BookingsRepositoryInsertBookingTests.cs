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
    public class BookingsRepositoryInsertBookingTests
    {
        public BookingsRepositoryInsertBookingTests()
        {
            //InitContext();
        }

        private VacationRentalDbContext _vacationalRentalDbContext;

        private IEnumerable<BookingEntity> BookingEntitiesArrange { get; set; }

        internal void InitContext()
        {
            var builder = new DbContextOptionsBuilder<VacationRentalDbContext>().UseInMemoryDatabase(databaseName: "VacationalRentalAPI2");

            var context = new VacationRentalDbContext(builder.Options);

            BookingEntitiesArrange = Enumerable.Range(1, 10)
                .Select(i => new BookingEntity { RentalId = 1, Nights = i, Start = DateTime.Now.Date, Unit = i });

            context.BookingEntities.AddRange(BookingEntitiesArrange);
            int changed = context.SaveChanges();
            _vacationalRentalDbContext = context;
        }

        [Fact]
        public async Task InsertBooking()
        {
            InitContext();

            var bookingEntity = new BookingEntity { Nights = 5, RentalId = 2, Start = new DateTime(2021, 04, 01) };

            var bookingRepository = new BookingsRepository(_vacationalRentalDbContext);

            var result = await bookingRepository.InsertBooking(bookingEntity);

            Assert.Equal(11, result);
        }
    }
}
