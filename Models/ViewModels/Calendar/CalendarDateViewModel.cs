namespace Models.ViewModels.Calendar;

public sealed class CalendarDateViewModel
{
    public CalendarDateViewModel(DateTime date, List<CalendarBookingViewModel> bookings, List<CalendarUnitViewModel> preparationTimes)
    {
        Date = date;
        Bookings = bookings;
        PreparationTimes = preparationTimes;
    }

    public DateTime Date { get; set; }
    public List<CalendarBookingViewModel> Bookings { get; set; }
    public List<CalendarUnitViewModel> PreparationTimes { get; set; }
}
