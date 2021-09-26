using System;
using System.Threading.Tasks;
using FakeItEasy;
using VacationRental.Domain.Exceptions;
using VacationRental.Domain.Values;
using VacationRental.Infrastructure.Persist.PersistModels;
using VacationRental.Infrastructure.Persist.Repositories;
using VacationRental.Infrastructure.Persist.Storage;
using Xunit;

namespace VacationRental.UnitTests.Infrastructure
{
    public class BookingRepositoryTests
    {
        [Fact]
        public async Task Get_RequestNotExistingBooking_BookingNotFoundExceptionThrown()
        {
            var repository = new BookingRepository(new InMemoryDataStorage<BookingDataModel>(model => model.Id));

            await Assert.ThrowsAsync<BookingNotFoundException>(async () => await repository.Get(new BookingId(1)));
        }

        [Fact]
        public async Task Get_RequestExistingBooking_BookingReturned()
        {
            var fake = A.Fake<IInMemoryDataStorage<BookingDataModel>>();

            var bookingId = 1;
            var rentalId = 1;
            var bookingDataModel = new BookingDataModel
            {
                Id = bookingId,
                Period = new TimePeriodDataModel {Days = 5, Start = new DateTime(2001, 1, 1)},
                PreparationInDays = 3,
                RentalId = rentalId,
                Unit = 1
            };
            

            A.CallTo(() => fake.TryGetValue(bookingDataModel.Id, out bookingDataModel)).Returns(true);

            var repository = new BookingRepository(fake);

            var booking = await repository.Get(new BookingId(bookingId));

            Assert.Equal(new BookingId(1), booking.Id);
            Assert.Equal(new RentalId(rentalId), booking.RentalId);
            Assert.Equal(bookingDataModel.Unit, booking.Unit);
            Assert.Equal(bookingDataModel.Period.Start, booking.Period.Start);
            Assert.Equal(bookingDataModel.Period.Days, booking.Period.Nights); // 
            Assert.Equal(bookingDataModel.PreparationInDays, booking.Preparation.Days);
        }
    }
}
