namespace VacationRental.Services.Dto;

public class CalendarDto
{
    public int RentalId { get; set; }
    public List<CalendarDateDto> Dates { get; set; } = new List<CalendarDateDto>();
}