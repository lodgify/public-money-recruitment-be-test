using AutoFixture;
using AutoMapper;
using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using VacationRental.Api.Models;
using VacationRental.Business;
using VacationRental.Business.BusinessLogic;
using VacationRental.Infrastructure.Models;
using VacationRental.Infrastructure.Repositories;
using Xunit;

namespace VacationRental.Api.Tests
{
    public class GetCalendarTests
    {
        private IFixture _fixture;
        private RentalBusinessLogic _rentalBusinessLogic;
        private BookingBusinessLogic _bookingBusinessLogic;
        private CalendarBusinessLogic _calendarBusinessLogic;
        public GetCalendarTests()
        {
            _fixture = new Fixture();

            _fixture.Inject<IRentalRepository>(new RentalRepository(new Dictionary<int, Rental>()));
            _fixture.Inject<IBookingsRepository>(new BookingRepository(new Dictionary<int, Booking>()));

            var mappingConfig = new MapperConfiguration(
                    mc => mc.AddProfile(new MappingProfile()));
            var mapper = mappingConfig.CreateMapper();

            _fixture.Inject(mapper);
            _rentalBusinessLogic = _fixture.Create<RentalBusinessLogic>();
            _bookingBusinessLogic = _fixture.Create<BookingBusinessLogic>();
            _calendarBusinessLogic = _fixture.Create<CalendarBusinessLogic>();
        }

        [Fact]
        public void GivenCompleteRequest_WhenGetCalendar_ThenAGetReturnsTheCalculatedCalendar()
        {
            // Arrange
            var postRentalRequest = _fixture.Build<RentalBindingModel>()
                .With(m => m.Units, 3)
                .With(m => m.PreparationTimeInDays, 2)
                .Create();

            int rentalId = _rentalBusinessLogic.AddRental(postRentalRequest);
            var postBooking1Request = _fixture.Build<BookingBindingModel>()
                .With(b => b.RentalId, rentalId)
                .With(b => b.Nights, 2)
                .With(b => b.Start, new DateTime(2000, 01, 02))
                .Create();

            var firstBooking = _bookingBusinessLogic.AddBooking(postBooking1Request);
            var postBooking2Request =
                _fixture.Build<BookingBindingModel>()
                .With(b => b.RentalId, rentalId)
                .With(b => b.Nights, 2)
                .With(b => b.Start, new DateTime(2000, 01, 03))
                .Create();

            var secondBooking = _bookingBusinessLogic.AddBooking(postBooking2Request);

            // Act
            var getCalendarResult = _calendarBusinessLogic.GetRentalCalendar(rentalId, new DateTime(2000, 1, 1), 5);

            // Assert
            Assert.Equal(rentalId, getCalendarResult.RentalId);
            Assert.Equal(5, getCalendarResult.Dates.Count);

            Assert.Equal(new DateTime(2000, 01, 01), getCalendarResult.Dates[0].Date);
            Assert.Empty(getCalendarResult.Dates[0].Bookings);

            Assert.Equal(new DateTime(2000, 01, 02), getCalendarResult.Dates[1].Date);
            Assert.Single(getCalendarResult.Dates[1].Bookings);
            Assert.Contains(getCalendarResult.Dates[1].Bookings, x => x.Id == firstBooking.Id);
            Assert.NotEqual(0, getCalendarResult.Dates[1].Bookings[0].Unit);

            Assert.Equal(new DateTime(2000, 01, 03), getCalendarResult.Dates[2].Date);
            Assert.Equal(2, getCalendarResult.Dates[2].Bookings.Count);
            Assert.Contains(getCalendarResult.Dates[2].Bookings, x => x.Id == firstBooking.Id);
            Assert.Contains(getCalendarResult.Dates[2].Bookings, x => x.Id == secondBooking.Id);
            Assert.NotEqual(0, getCalendarResult.Dates[2].Bookings[0].Unit);
            Assert.NotEqual(0, getCalendarResult.Dates[2].Bookings[1].Unit);


            Assert.Equal(new DateTime(2000, 01, 04), getCalendarResult.Dates[3].Date);
            Assert.Single(getCalendarResult.Dates[3].Bookings);
            Assert.Contains(getCalendarResult.Dates[3].Bookings, x => x.Id == secondBooking.Id);
            Assert.NotEqual(0, getCalendarResult.Dates[3].Bookings[0].Unit);

            Assert.Equal(new DateTime(2000, 01, 05), getCalendarResult.Dates[4].Date);
            Assert.Empty(getCalendarResult.Dates[4].Bookings);
            Assert.NotEmpty(getCalendarResult.Dates[4].PreparationTimes);

        }

        [Fact]
        public void Calendar_WhenUnitMiddleUnitIsAvailable_ThenItShouldBeAssignedForNextBooking()
        {
            // Arrange
            var rental = _fixture.Build<Rental>()
                .With(m => m.Id, 1)
                .With(m => m.Units, 3)
                .With(m => m.PreparationTimeInDays, 1)
                .Create();

            Dictionary<int, Rental> rentalRepo = new Dictionary<int, Rental>() { { rental.Id, rental } };
            _fixture.Inject<IRentalRepository>(new RentalRepository(rentalRepo));

            var booking1 = _fixture.Build<Booking>()
                .With(m => m.Id, 1)
                .With(b => b.RentalId, rental.Id)
                .With(b => b.Nights, 3)
                .With(b => b.Start, new DateTime(2000, 01, 03))
                .Create();

            var booking2 = _fixture.Build<Booking>()
                .With(m => m.Id, 2)
                .With(b => b.RentalId, rental.Id)
                .With(b => b.Nights, 1)
                .With(b => b.Start, new DateTime(2000, 01, 02))
                .Create();

            var booking3 = _fixture.Build<Booking>()
                .With(m => m.Id, 3)
                .With(b => b.RentalId, rental.Id)
                .With(b => b.Nights, 3)
                .With(b => b.Start, new DateTime(2000, 01, 03))
                .Create();

            var booking4 = _fixture.Build<Booking>()
                .With(m => m.Id, 4)
                .With(b => b.RentalId, rental.Id)
                .With(b => b.Nights, 2)
                .With(b => b.Start, new DateTime(2000, 01, 05))
                .Create();

            Dictionary<int, Booking> bookingRepo = new Dictionary<int, Booking>() 
            {
                { booking1.Id, booking1 },
                { booking2.Id, booking2 },
                { booking3.Id, booking3 },
                { booking4.Id, booking4 },
            };

            _fixture.Inject<IBookingsRepository>(new BookingRepository(bookingRepo));
            _calendarBusinessLogic = _fixture.Create<CalendarBusinessLogic>();

            // Act
            var calendar = _calendarBusinessLogic.GetRentalCalendar(rental.Id, new DateTime(2000, 01, 02), 6);

            // Assert
            var booking = calendar.Dates[3].Bookings[2];
            Assert.Equal(2, booking.Unit);
        }

        [Fact]
        public void Calendar_WhenUnitIsInPreparation_ThenItShouldBeUnavailable()
        {
            // Arrange
            var rental = _fixture.Build<Rental>()
                .With(m => m.Id, 1)
                .With(m => m.Units, 3)
                .With(m => m.PreparationTimeInDays, 1)
                .Create();

            Dictionary<int, Rental> rentalRepo = new Dictionary<int, Rental>() { { rental.Id, rental } };
            _fixture.Inject<IRentalRepository>(new RentalRepository(rentalRepo));

            var booking1 = _fixture.Build<Booking>()
                .With(m => m.Id, 1)
                .With(b => b.RentalId, rental.Id)
                .With(b => b.Nights, 1)
                .With(b => b.Start, new DateTime(2000, 01, 02))
                .Create();

            var booking2 = _fixture.Build<Booking>()
                .With(m => m.Id, 2)
                .With(b => b.RentalId, rental.Id)
                .With(b => b.Nights, 1)
                .With(b => b.Start, new DateTime(2000, 01, 04))
                .Create();

            Dictionary<int, Booking> bookingRepo = new Dictionary<int, Booking>()
            {
                { booking1.Id, booking1 },
                { booking2.Id, booking2 },
            };

            _fixture.Inject<IBookingsRepository>(new BookingRepository(bookingRepo));
            _calendarBusinessLogic = _fixture.Create<CalendarBusinessLogic>();

            // Act
            var calendar = _calendarBusinessLogic.GetRentalCalendar(rental.Id, new DateTime(2000, 01, 02), 5);

            // Assert
            var booking = calendar.Dates[2].Bookings[0];
            Assert.Equal(2, booking.Unit);
        }
    }
}
