using System;
using FakeItEasy;
using NUnit.Framework;
using VacationRental.Api.Models;
using VacationRental.Api.Tests.Stubs;

namespace VacationRental.Api.Tests.Units
{
    public class BookingServiceTests : BaseServiceTests
    {
        [Test]
        public void ShouldReturnCorrectBookingViewModelOnGet()
        {
            int id = 1;
            var expectedResult = new BookingViewModel()
            {
                Id = 1,
                Start = DateTime.Today,
                Nights = 3,
                Unit = 1
            };

            A.CallTo(() => BookingRepository.HasValue(id)).Returns(true);
            A.CallTo(() => BookingRepository.Get(id)).Returns(expectedResult);

            var booking = BookingService.Get(id);

            Assert.That(booking, Is.Not.Null);
            Assert.AreEqual(expectedResult, booking);
        }

        [Test]
        public void ShouldThrowApplicationExceptionIfBookingNotFoundOnGet()
        {
            A.CallTo(() => BookingRepository.HasValue(A<int>._)).Returns(false);

            var ex = Assert.Throws<ApplicationException>(() => BookingService.Get(3));

            A.CallTo(() => BookingRepository.Get(A<int>._)).MustNotHaveHappened();
            Assert.That(ex, Is.Not.Null);
            Assert.That(ex.Message, Is.EqualTo("Booking not found"));
        }

        [TestCase(0)]
        [TestCase(-1)]
        public void ShouldThrowApplicationExceptionIfNightsIsNotPositiveOnCreation(int nights)
        {
            var booking = new BookingBindingModel()
            {
                Start = DateTime.Today,
                Nights = nights,
                RentalId = 1
            };

            var ex = Assert.Throws<ApplicationException>(() => BookingService.Create(booking));

            A.CallTo(() => BookingRepository.Add(A<int>._, A<BookingViewModel>._)).MustNotHaveHappened();
            Assert.That(ex, Is.Not.Null);
            Assert.That(ex.Message, Is.EqualTo("Nights must be positive"));
        }

        [Test]
        public void ShouldThrowApplicationExceptionIfStartDateIsNotInFutureOnCreation()
        {
            var booking = new BookingBindingModel()
            {
                Start = new DateTime(2022, 7, 9),
                Nights = 3,
                RentalId = 1
            };

            var ex = Assert.Throws<ApplicationException>(() => BookingService.Create(booking));

            A.CallTo(() => BookingRepository.Add(A<int>._, A<BookingViewModel>._)).MustNotHaveHappened();
            Assert.That(ex, Is.Not.Null);
            Assert.That(ex.Message, Is.EqualTo("Booking must be in future"));
        }

        [Test]
        public void ShouldThrowApplicationExceptionIfRentalNotFoundOnCreation()
        {
            var booking = new BookingBindingModel()
            {
                Start = new DateTime(2022, 9, 9),
                Nights = 3,
                RentalId = 1
            };

            A.CallTo(() => RentalRepository.HasValue(A<int>._)).Returns(false);

            var ex = Assert.Throws<ApplicationException>(() => BookingService.Create(booking));

            RentalNotFoundExceptionChecks(ex);
        }

        [Test]
        public void ShouldThrowApplicationExceptionIfNotAvailableUnitOnCreation()
        {
            var booking = new BookingBindingModel()
            {
                Start = new DateTime(2022, 9, 9),
                Nights = 3,
                RentalId = 1
            };

            var rental = new RentalViewModel()
            {
                PreparationTimeInDays = 2,
                Units = 2,
                Id = 1
            };

            A.CallTo(() => RentalRepository.HasValue(A<int>._)).Returns(true);
            A.CallTo(() => BookingRepository.GetBookingsByRentalId(booking.RentalId)).Returns(BookingStubs.BookingWithCrossDays());
            A.CallTo(() => RentalRepository.Get(booking.RentalId)).Returns(rental);


            var ex = Assert.Throws<ApplicationException>(() => BookingService.Create(booking));

            A.CallTo(() => BookingRepository.Add(A<int>._, A<BookingViewModel>._)).MustNotHaveHappened();
            Assert.That(ex, Is.Not.Null);
            Assert.That(ex.Message, Is.EqualTo("Not available"));
        }

        [Test]
        public void ShouldCreateNewBookingIfDataValidOnCreation()
        {
            var booking = new BookingBindingModel()
            {
                Start = new DateTime(2022, 9, 9),
                Nights = 3,
                RentalId = 1
            };

            var rental = new RentalViewModel()
            {
                PreparationTimeInDays = 2,
                Units = 2,
                Id = 1
            };

            var bookings = BookingStubs.BookingsWithoutCrossDays();

            A.CallTo(() => RentalRepository.HasValue(A<int>._)).Returns(true);
            A.CallTo(() => BookingRepository.GetBookingsByRentalId(booking.RentalId)).Returns(bookings);
            A.CallTo(() => RentalRepository.Get(booking.RentalId)).Returns(rental);
            A.CallTo(() => BookingRepository.Count).Returns(3);

            var key = BookingService.Create(booking);

            A.CallTo(() => BookingRepository.Add(A<int>._, A<BookingViewModel>._)).MustHaveHappened();
            Assert.That(key, Is.Not.Null);
            Assert.AreEqual(4, key.Id);
        }
    }
}
