using System;
using System.Threading.Tasks;
using FakeItEasy;
using VacationRental.Domain.Entities;
using VacationRental.Domain.Exceptions;
using VacationRental.Domain.Values;
using VacationRental.Infrastructure.Persist.PersistModels;
using VacationRental.Infrastructure.Persist.Repositories;
using VacationRental.Infrastructure.Persist.Storage;
using Xunit;

namespace VacationRental.UnitTests.Infrastructure
{
    [Collection("Infrastructure")]
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

        [Fact]
        public async Task Update_NotExistingBooking_BookingNotFoundExceptionThrown()
        {
            var fakeStorage = A.Fake<IInMemoryDataStorage<BookingDataModel>>();
            var empty = new BookingDataModel();
            A.CallTo(() => fakeStorage.TryGetValue(A<int>._, out empty)).Returns(false);

            var repository = new BookingRepository(fakeStorage);

            //Act
            await Assert.ThrowsAsync<BookingNotFoundException>(async () =>
                await repository.Update(new Booking(new BookingId(1), new RentalId(1),
                    new BookingPeriod(new DateTime(2001, 1, 1), 1), 1, 1)));
        }

        [Fact]
        public async Task Update_ExistingBooking_StoredSuccessfully()
        {
            var id = 1;
            var rentalId = 1;
            var fakeStorage = A.Fake<IInMemoryDataStorage<BookingDataModel>>();
            var modelBeforeUpdating = new BookingDataModel
            {
                Id = id, RentalId = rentalId,
                Period = new TimePeriodDataModel {Start = new DateTime(2001, 1, 1), Days = 1}
            };

            var unit = 2;
            var preparationTime = 2;
            var start = new DateTime(2001, 2,1);
            var nights = 1;
            var booking = new Booking(new BookingId(id), new RentalId(rentalId), new BookingPeriod(start, nights), preparationTime, unit);

            A.CallTo(() => fakeStorage.TryGetValue(id, out modelBeforeUpdating)).Returns(true);

            var repository = new BookingRepository(fakeStorage);

            //Act
            await repository.Update(booking);


            A.CallTo(() => fakeStorage.Update(A<BookingDataModel>.That.Matches(param => param.Id == id
                                                                                       && param.Unit == unit
                                                                                       && param.PreparationInDays == preparationTime
                                                                                       && param.RentalId == rentalId
                                                                                       && param.Period.Start == start
                                                                                       && param.Period.Days == nights))
                ).MustHaveHappenedOnceExactly();
        }
    }
}
