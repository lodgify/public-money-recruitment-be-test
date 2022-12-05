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
    public class PostBookingTests
    {
        private readonly IFixture _fixture;
        private readonly BookingBusinessLogic _bookingBusinessLogic;
        private readonly RentalBusinessLogic _rentalBusinessLogic;

        public PostBookingTests()
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
        }

        [Fact]
        public void GivenCompleteRequest_WhenPostBooking_ThenAGetReturnsTheCreatedBooking()
        {
            // Arrange
            var postRentalRequest = _fixture.Build<RentalBindingModel>()
                .With(m => m.Units, 4)
                .With(m => m.PreparationTimeInDays, 1)
                .Create();

            int rentalId = _rentalBusinessLogic.AddRental(postRentalRequest);
            var postBookingRequest = _fixture.Build<BookingBindingModel>()
                .With(m => m.RentalId, rentalId)
                .With(m => m.Nights, 3)
                .With(m => m.Start, new DateTime(2001, 01, 01))
                .Create();

            // Act
            var savedBooking = _bookingBusinessLogic.AddBooking(postBookingRequest);
            var booking = _bookingBusinessLogic.GetBooking(savedBooking.Id);

            // Assert
            Assert.Equal(postBookingRequest.RentalId, booking.RentalId);
            Assert.Equal(postBookingRequest.Nights, booking.Nights);
            Assert.Equal(postBookingRequest.Start, booking.Start);
        }

        [Fact]
        public void GivenCompleteRequest_WhenPostBooking_ThenAPostReturnsErrorWhenThereIsOverbooking()
        {
            // Arrange
            var postRentalRequest = _fixture.Build<RentalBindingModel>()
                .With(m => m.Units, 1)
                .Create();

            int rentalId = _rentalBusinessLogic.AddRental(postRentalRequest);

            var postBooking1Request = _fixture.Build<BookingBindingModel>()
                .With(m => m.RentalId, rentalId)
                .With(m => m.Nights, 3)
                .With(m => m.Start, new DateTime(2002, 01, 01))
                .Create();

            // Act
            _bookingBusinessLogic.AddBooking(postBooking1Request);

            var postBooking2Request = _fixture.Build<BookingBindingModel>()
                .With(m => m.RentalId, rentalId)
                .With(m => m.Nights, 3)
                .With(m => m.Start, new DateTime(2002, 01, 02))
                .Create();

            // Assert
            Assert.Throws<ApplicationException>(() => _bookingBusinessLogic.AddBooking(postBooking2Request));
        }

        [Fact]
        public void Booking_WhenAllUnitsAreBooking_ShouldThrowException()
        {
            // Arrange
            var postRentalRequest = _fixture.Build<RentalBindingModel>()
                .With(m => m.Units, 1)
                .Create();

            int rentalId = _rentalBusinessLogic.AddRental(postRentalRequest);

            var booking1 = _fixture.Build<BookingBindingModel>()
                .With(m => m.RentalId, rentalId)
                .With(m => m.Nights, 1)
                .With(m => m.Start, new DateTime(2002, 01, 01))
                .Create();

            _bookingBusinessLogic.AddBooking(booking1);

            var booking2 = _fixture.Build<BookingBindingModel>()
                .With(m => m.RentalId, rentalId)
                .With(m => m.Nights, 1)
                .With(m => m.Start, new DateTime(2002, 01, 01))
                .Create();

            // Act, Assert
            Assert.Throws<ApplicationException>(() => _bookingBusinessLogic.AddBooking(booking2));
        }

        [Fact]
        public void Booking_WhenVacantUnitIsInPreparation_ShouldThrowException()
        {
            // Arrange
            var postRentalRequest = _fixture.Build<RentalBindingModel>()
                .With(m => m.Units, 1)
                .Create();

            int rentalId = _rentalBusinessLogic.AddRental(postRentalRequest);

            var booking1 = _fixture.Build<BookingBindingModel>()
                .With(m => m.RentalId, rentalId)
                .With(m => m.Nights, 1)
                .With(m => m.Start, new DateTime(2002, 01, 01))
                .Create();

            _bookingBusinessLogic.AddBooking(booking1);

            var booking2 = _fixture.Build<BookingBindingModel>()
                .With(m => m.RentalId, rentalId)
                .With(m => m.Nights, 1)
                .With(m => m.Start, new DateTime(2002, 01, 02))
                .Create();

            // Act, Assert
            Assert.Throws<ApplicationException>(() => _bookingBusinessLogic.AddBooking(booking2));
        }

    }
}
