using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using VacationRental.Api.Models;

namespace VacationRental.Api.Validation
{
    public class PositiveNightHandler:AbstractValidationHandler
    {
        public override void Handle(BookingBindingModel model, IDictionary<int, RentalViewModel> rentals,
            IDictionary<int, BookingViewModel> bookings)
        {
            if (model.Nights <= 0)
            {
                throw new ApplicationException("Nigts must be positive");
            }
            else
            {
                base.Handle(model, rentals, bookings);
            }
        }
    }
}
