using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using VacationRental.Api.Models;

namespace VacationRental.Api.Validation
{
    public class RentalAvailableHandler : AbstractValidationHandler
    {
        public override void Handle(BookingBindingModel model, IDictionary<int, RentalViewModel> rentals,
            IDictionary<int, BookingViewModel> bookings)
        {
            for (var i = 0; i < model.Nights; i++)
            {
                var count = 0;
                foreach (var booking in bookings.Values)
                {
                    if (booking.RentalId == model.RentalId
                        && (booking.Start <= model.Start.Date && booking.Start.AddDays(booking.Nights) > model.Start.Date)
                        || (booking.Start < model.Start.AddDays(model.Nights) && booking.Start.AddDays(booking.Nights) >= model.Start.AddDays(model.Nights))
                        || (booking.Start > model.Start && booking.Start.AddDays(booking.Nights) < model.Start.AddDays(model.Nights)))
                    {
                        count++;
                    }
                }
                if (count >= rentals[model.RentalId].Units)
                {
                    throw new ApplicationException("Not available");
                }
                else
                {
                    base.Handle(model, rentals, bookings);
                }
            }
        }
    }
}
