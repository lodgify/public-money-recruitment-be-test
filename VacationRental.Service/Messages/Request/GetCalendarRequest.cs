using System;

namespace VacationRental.Application
{
    public class GetCalendarRequest
    {
        public int rentalId { get; set; }
        public DateTime bookingStartDate { get; set; }
        public int numberOfnights { get; set; }
    }
}
