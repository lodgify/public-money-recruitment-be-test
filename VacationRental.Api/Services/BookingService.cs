using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using VacationRental.Api.Models;
using VacationRental.Api.Validation;

namespace VacationRental.Api.Services
{
    public class BookingService:IBookingService
    {
        public void bookingGetValidation(int bookingId, IDictionary<int, BookingViewModel> _bookings)
        {
            if (!_bookings.ContainsKey(bookingId))
                throw new ApplicationException("Booking not found");
        }
        public ResourceIdViewModel addResourceIdViewModel(BookingBindingModel model,
            IDictionary<int, RentalViewModel> _rentals,
            IDictionary<int, BookingViewModel> _bookings)
        {
            ResourceIdViewModel key = addResourceValidation(model, _rentals, _bookings);
            _bookings.Add(key.Id, new BookingViewModel
            {
                Id = key.Id,
                Nights = model.Nights,
                RentalId = model.RentalId,
                Start = model.Start.Date
            });
            return key;
        }

        private static ResourceIdViewModel addResourceValidation(BookingBindingModel model, IDictionary<int, RentalViewModel> _rentals, IDictionary<int, BookingViewModel> _bookings)
        {
            var positiveNightHandler = new PositiveNightHandler();
            var rentalExistHandler = new RentalExistHandler();
            var rentalAvailableHandler = new RentalAvailableHandler();
            positiveNightHandler.SetNext(rentalExistHandler).SetNext(rentalAvailableHandler);
            positiveNightHandler.Handle(model, _rentals, _bookings);
            var key = new ResourceIdViewModel { Id = _bookings.Keys.Count + 1 };
            return key;
        }
    }
}
