using System;
using System.Collections.Generic;
using System.Text;
using VacationRental.Domain.Models;

namespace VacationRental.Domain.Translator.Booking
{
    public static class Translator
    {
        public static BookingViewModel ViewModelTranslator(this BookingBindingModel model)
        {
            var bookingViewModel = new BookingViewModel
            {
                Nights = model.Nights,
                RentalId = model.RentalId,
                Start = model.Start
            };

            return bookingViewModel;
        }
    }
}
