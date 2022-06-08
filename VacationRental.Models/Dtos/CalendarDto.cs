namespace VacationRental.Models.Dtos
{
    public class CalendarDto
    {
        public int RentalId { get; set; }
        public CalendarDateDto[]? Dates { get; set; }
    }
}
