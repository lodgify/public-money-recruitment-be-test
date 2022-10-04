namespace VacationRental.Application.Calendars.Models
{
    public class CalendarBookingViewModel
    {
        public CalendarBookingViewModel(int id, int unit)
        {
            Id = id;
            Unit = unit;
        }

        public int Id { get; set; }
        public int Unit { get; set; }
    }
}
