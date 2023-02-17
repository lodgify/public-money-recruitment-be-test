using Models.ViewModels;

namespace VacationRental.Api.Operations.BookingOperations
{
    public sealed class BookingCreateOperation : IBookingCreateOperation
    {
        public BookingCreateOperation()
        {
        }

        public ResourceIdViewModel ExecuteAsync(BookingBindingViewModel model)
        {
            if (model.Nights <= 0)
                throw new ApplicationException("Nigts must be positive");

            return DoExecute(model);
        }

        private ResourceIdViewModel DoExecute(BookingBindingViewModel model)
        {
            if (!_rentals.ContainsKey(model.RentalId))
                throw new ApplicationException("Rental not found");


            for (var i = 0; i < model.Nights; i++)
            {
                var count = 0;
                foreach (var booking in _bookings.Values)
                {
                    if (booking.RentalId == model.RentalId
                        && (booking.Start <= model.Start.Date && booking.Start.AddDays(booking.Nights) > model.Start.Date)
                        || (booking.Start < model.Start.AddDays(model.Nights) &&
                            booking.Start.AddDays(booking.Nights) >= model.Start.AddDays(model.Nights))
                        || (booking.Start > model.Start &&
                            booking.Start.AddDays(booking.Nights) < model.Start.AddDays(model.Nights)))
                    {
                        count++;
                    }
                }

                if (count >= _rentals[model.RentalId].Units)
                    throw new ApplicationException("Not available");
            }


            var key = new ResourceIdViewModel { Id = _bookings.Keys.Count + 1 };

            _bookings.Add(key.Id, new BookingViewModel
            {
                Id = key.Id,
                Nights = model.Nights,
                RentalId = model.RentalId,
                Start = model.Start.Date
            });

            return key;
        }
    }
}
