using System.Collections;
using System.Collections.Generic;
using System.Linq;
using VacationRental.Api.Models;

namespace VacationRental.Api.Helpers
{
    public static class BookingHelper
    {
        public static bool CheckAvailability(IEnumerable<BookingViewModel> bookings, RentalViewModel rental)
        {
            foreach (var booking in bookings.ToList())
            {
                if(CheckAvailability(booking, bookings, rental))
                {
                    return false;
                }
            }

            return true;
        }
        
        public static bool CheckAvailability(BookingViewModel model, IEnumerable<BookingViewModel> bookings, RentalViewModel rental)
        {
            for (var i = 0; i < model.Nights; i++)
            {
                var count = 0;
                foreach (var booking in bookings)
                {
                    if (booking.RentalId == model.RentalId
                        && (booking.Start <= model.Start.Date && booking.Start.AddDays(booking.Nights + rental.PreparationTimeInDays) > model.Start.Date)
                        || (booking.Start < model.Start.AddDays(model.Nights) && booking.Start.AddDays(booking.Nights + rental.PreparationTimeInDays) >= model.Start.AddDays(model.Nights))
                        || (booking.Start > model.Start && booking.Start.AddDays(booking.Nights + rental.PreparationTimeInDays) < model.Start.AddDays(model.Nights)))
                    {
                        count++;
                    }
                }

                if (count >= rental.Units)
                    return false;
            }
            return true;
        }
    }
}