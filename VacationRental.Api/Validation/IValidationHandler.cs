using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using VacationRental.Api.Models;

namespace VacationRental.Api.Validation
{
    public interface IValidationHandler
    {
        IValidationHandler SetNext(IValidationHandler handler);

        void Handle(BookingBindingModel model, IDictionary<int, RentalViewModel> rentals
            , IDictionary<int, BookingViewModel> bookings);
    }
}
