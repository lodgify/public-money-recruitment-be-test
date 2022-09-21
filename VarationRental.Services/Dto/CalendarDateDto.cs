namespace VacationRental.Services.Dto;

public class CalendarDateDto
{
    public DateTime Date { get; set; }
    public List<CalendarBookingDto> Bookings { get; set; } = new List<CalendarBookingDto>();
}