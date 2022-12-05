using AutoFixture;
using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using VacationRental.Api.Models;
using VacationRental.Business;
using VacationRental.Business.BusinessLogic;
using VacationRental.Infrastructure.Models;
using VacationRental.Infrastructure.Repositories;
using Xunit;

namespace VacationRental.Api.Tests
{
    public class PutRentalTests
    {
        private IFixture _fixture;
        private RentalBusinessLogic _rentalBusinessLogic;
        private BookingBusinessLogic _bookingBusinessLogic;
        private IBookingsRepository _bookingsRepository;
        public PutRentalTests()
        {
            _fixture = new Fixture();

            _fixture.Inject<IRentalRepository>(new RentalRepository(new Dictionary<int, Rental>()));
            _fixture.Inject<IRentalRepository>(new RentalRepository(new Dictionary<int, Rental>()));
            _fixture.Inject<IBookingsRepository>(new BookingRepository(new Dictionary<int, Booking>()));

            var mappingConfig = new MapperConfiguration(
                    mc => mc.AddProfile(new MappingProfile()));
            var mapper = mappingConfig.CreateMapper();

            _fixture.Inject(mapper);

            _bookingBusinessLogic = _fixture.Create<BookingBusinessLogic>();
            _rentalBusinessLogic = _fixture.Create<RentalBusinessLogic>();
        }

        [Fact]
        public void RentalUpdate_ShouldSucceed_WhenUpdateWithNoBooking()
        {
            // Assert
            var oldRental = _fixture.Create<RentalBindingModel>();

            int rentalId = _rentalBusinessLogic.AddRental(oldRental);
            var newRental = _fixture.Create<RentalBindingModel>();

            // Act
            var updatedRental = _rentalBusinessLogic.UpdateRental(rentalId, newRental);

            // Assert
            Assert.Equal(rentalId, updatedRental.Id);
            Assert.Equal(newRental.Units, updatedRental.Units);
            Assert.Equal(newRental.PreparationTimeInDays, updatedRental.PreparationTimeInDays);
        }

        [Fact]
        public void RentalUpdate_ShouldSucceed_WhenUnitsIncreaseAndPreparationDecrease()
        {
            // Assert
            var oldRental = _fixture.Build<RentalBindingModel>()
                .With(rental => rental.Units, 3)
                .With(rental => rental.PreparationTimeInDays, 3)
                .Create();

            int rentalId = _rentalBusinessLogic.AddRental(oldRental);

            IEnumerable<BookingBindingModel> bookings = _fixture.Build<BookingBindingModel>()
                .With(booking => booking.RentalId, rentalId)
                .With(booking => booking.Start, DateTime.Now.Date)
                .CreateMany();

            foreach (var booking in bookings)
                _bookingBusinessLogic.AddBooking(booking);

            var newRental = _fixture.Build<RentalBindingModel>()
                .With(rental => rental.Units, 4)
                .With(rental => rental.PreparationTimeInDays, 2)
                .Create();

            // Act
            var updatedRental = _rentalBusinessLogic.UpdateRental(rentalId, newRental);

            // Assert
            Assert.Equal(rentalId, updatedRental.Id);
            Assert.Equal(newRental.Units, updatedRental.Units);
            Assert.Equal(newRental.PreparationTimeInDays, updatedRental.PreparationTimeInDays);
        }

        [Fact]
        public void RentalUpdate_ShouldThrowException_WhenUnitsDecreaseAndBookingsDontHaveEnoughUnits()
        {
            // Assert
            var oldRental = _fixture.Build<RentalBindingModel>()
                .With(rental => rental.Units, 2)
                .Create();

            int rentalId = _rentalBusinessLogic.AddRental(oldRental);

            IEnumerable<BookingBindingModel> bookings = _fixture.Build<BookingBindingModel>()
                .With(booking => booking.RentalId, rentalId)
                .With(booking => booking.Start, DateTime.Now.Date)
                .CreateMany(2);

            List<BookingViewModel> addedBookings = new List<BookingViewModel>();

            foreach (var booking in bookings)
                addedBookings.Add(_bookingBusinessLogic.AddBooking(booking));

            var newRental = _fixture.Build<RentalBindingModel>()
                .With(rental => rental.Units, 1)
                .Create();

            // Act, Assert
            Assert.Throws<ApplicationException>(() => _rentalBusinessLogic.UpdateRental(rentalId, newRental));
            Assert.Equal(2, _rentalBusinessLogic.GetRental(rentalId).Units);

            foreach (var booking in addedBookings)
                _bookingBusinessLogic.GetBooking(booking.Id);
        }

        [Fact]
        public void RentalUpdate_ShouldThrowException_WhenPreparationTimeIncreasAndBookingsConflict()
        {
            // Assert
            var oldRental = _fixture.Build<RentalBindingModel>()
                .With(rental => rental.Units, 1)
                .With(rental => rental.PreparationTimeInDays, 1)
                .Create();

            int rentalId = _rentalBusinessLogic.AddRental(oldRental);

            var booking1 = _fixture.Build<BookingBindingModel>()
                .With(booking => booking.RentalId, rentalId)
                .With(booking => booking.Nights, 1)
                .With(booking => booking.Start, DateTime.Now.Date)
                .Create();

            var booking2 = _fixture.Build<BookingBindingModel>()
                .With(booking => booking.RentalId, rentalId)
                .With(booking => booking.Start, DateTime.Now.Date.AddDays(4))
                .Create();

            var booking1Response = _bookingBusinessLogic.AddBooking(booking1);
            var booking2Response = _bookingBusinessLogic.AddBooking(booking2);

            var newRental = _fixture.Build<RentalBindingModel>()
                .With(rental => rental.Units, 1)
                .With(rental => rental.PreparationTimeInDays, 5)
                .Create();

            // Act, Assert
            Assert.Throws<ApplicationException>(() => _rentalBusinessLogic.UpdateRental(rentalId, newRental));
            Assert.Equal(1, _rentalBusinessLogic.GetRental(rentalId).PreparationTimeInDays);

            _bookingBusinessLogic.GetBooking(booking1Response.Id);
            _bookingBusinessLogic.GetBooking(booking2Response.Id);
        }


    }
}
