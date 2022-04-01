using System.Collections.Generic;
using VacationRental.Domain.Bookings;
using VacationRental.Domain.Rentals;

namespace VacationRental.Application.Common.Services.ReCalculateBookingsService
{
    public interface IValidateRentalModificationService
    {
        bool Validate(RentalModel rental, IEnumerable<BookingModel> bookings);
    }
}
