namespace VacationRental.Infrastructure.DTOs
{
    public class CalendarDateDTO
    {
        public DateTime Date { get; set; }

        public List<CalendarBookingDTO> Bookings { get; set; }

        public List<CalendarPreparationTimeDTO> PreparationTimes { get; set; }
    }
}
