using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using VacationRental.Api.Models;

namespace VacationRental.Api.Services
{
    public interface IBookingService
    {
        void bookingGetValidation(int bookingId, IDictionary<int, BookingViewModel> _bookings);
        ResourceIdViewModel addResourceIdViewModel(BookingBindingModel model,
    IDictionary<int, RentalViewModel> _rentals,
    IDictionary<int, BookingViewModel> _bookings);
    }
}
