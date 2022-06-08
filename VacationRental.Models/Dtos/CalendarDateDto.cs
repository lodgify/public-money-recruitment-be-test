namespace VacationRental.Models.Dtos
{
    public class CalendarDateDto
    {
        public DateTime Date { get; set; }
        public CalendarBookingDto[]? Bookings { get; set; }
        public CalendarPreparationTimeDto[]? PreparationTimes { get; set; }
    }
}
