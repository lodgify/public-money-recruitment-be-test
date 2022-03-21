using VacationRental.Domain.Models.Interfaces;

namespace VacationRental.Domain.Models
{
    public class CalendarBooking : IBookingPeriod
    {
        public int Id { get; set; }

        public int Unit { get; set; }
    }
}
