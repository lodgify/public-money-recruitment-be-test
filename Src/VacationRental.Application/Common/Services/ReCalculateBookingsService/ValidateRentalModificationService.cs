using System.Collections.Generic;
using System.Linq;
using VacationRental.Application.Common.Services.BookingSearchService;
using VacationRental.Domain.Bookings;
using VacationRental.Domain.Rentals;

namespace VacationRental.Application.Common.Services.ReCalculateBookingsService
{
    public class ValidateRentalModificationService : IValidateRentalModificationService
    {
        private readonly IBookingSearchService _bookingSearchService;

        public ValidateRentalModificationService(IBookingSearchService bookingSearchService)
        {
            _bookingSearchService = bookingSearchService;
        }

        public bool Validate(RentalModel rental, IEnumerable<BookingModel> bookings)
        {
            foreach (var booking in bookings)
            {
                var bookingsFound = _bookingSearchService.GetBookingsByRangeOfTime(new GetBookingsByRangeOfTimeDTO()
                {
                    Bookings = bookings,
                    Nights = booking.Nights,
                    Start = booking.Start,
                    PreparationTime = rental.PreparationTimeInDays
                });

                if (bookingsFound.Count() > rental.Units)
                    return false;
            }

            return true;
        }
    }
}
