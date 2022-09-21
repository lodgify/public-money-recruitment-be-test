namespace VacationRental.Services.Dto;

public class CalendarFilterDto
{
    public int RentalId { get; set; }
    public DateTime Start { get; set; }
    public int Nights { get; set; }
}
