using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using VacationRental.Api.Models;

namespace VacationRental.Api.Validation
{
    public class RentalExistHandler : AbstractValidationHandler
    {
        public override void Handle(BookingBindingModel model,IDictionary<int, RentalViewModel> rentals,
            IDictionary<int, BookingViewModel> bookings)
        {
            if (!rentals.ContainsKey(model.RentalId))
            {
                throw new ApplicationException("Rental not found");
            }
            else
            {
                base.Handle(model,rentals,bookings);
            }
        }
    }
}
