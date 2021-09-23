using System;

namespace VacationRental.Application.Queries.Calendar
{
    public class BookingCalendarForRentalQuery
    {
        public int RentalId { get; set; }
        public DateTime Start { get; set; }
        public int Nights { get; set; }
    }
}
