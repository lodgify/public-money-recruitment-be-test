using Moq;
using VacationRental.Api.Controllers;
using VacationRental.Api.Interfaces;

namespace VacationRental.UnitTest
{
    public class BaseConfiguration
    {
        private  IRentalService _rentalService;
        private IBookingService _bookingService;
        private ICalendarService _calendarService;

        internal BaseConfiguration()
        {
            _calendarService = new Mock<ICalendarService>().Object;
            _bookingService = new Mock<IBookingService>().Object;
            _rentalService = new Mock<IRentalService>().Object;
        }
        
        
        internal BaseConfiguration WithRentalService(IRentalService rentalService)
        {
            _rentalService = rentalService;
            return this;
        }
        
        internal BaseConfiguration WithBookingService(IBookingService bookingService)
        {
            _bookingService = bookingService;
            return this;
        }
        
        internal BaseConfiguration WithCalendarService(ICalendarService calendarService)
        {
            _calendarService = calendarService;
            return this;
        }


        internal BookingsController BuildBookingsController() => new(_bookingService);
        
        internal RentalsController BuildRentalsController() => new(_rentalService);
        
        internal CalendarController BuildCalendarController() => new(_calendarService);



    }
}