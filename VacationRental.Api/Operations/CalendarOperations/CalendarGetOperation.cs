using Models.ViewModels;

namespace VacationRental.Api.Operations.CalendarOperations
{
    public sealed class CalendarGetOperation : ICalendarGetOperation
    {
        public CalendarGetOperation()
        {
        }

        public CalendarViewModel ExecuteAsync(int rentalId, DateTime start, int nights)
        {
            if (rentalId <= 0)
                throw new ApplicationException("Wrong Id"); 
            if (nights <= 0)
                throw new ApplicationException("Wrong nights");

            return DoExecute(rentalId, start, nights);
        }

        private CalendarViewModel DoExecute(int rentalId, DateTime start, int nights)
        {
            if (!_rentals.ContainsKey(rentalId))
                throw new ApplicationException("Rental not found");

            var result = new CalendarViewModel
            {
                RentalId = rentalId,
                Dates = new List<CalendarDateViewModel>()
            };
            for (var i = 0; i < nights; i++)
            {
                var date = new CalendarDateViewModel
                {
                    Date = start.Date.AddDays(i),
                    Bookings = new List<CalendarBookingViewModel>()
                };

                foreach (var booking in _bookings.Values)
                {
                    if (booking.RentalId == rentalId
                        && booking.Start <= date.Date && booking.Start.AddDays(booking.Nights) > date.Date)
                    {
                        date.Bookings.Add(new CalendarBookingViewModel { Id = booking.Id });
                    }
                }

                result.Dates.Add(date);
            }
        }
    }
}
