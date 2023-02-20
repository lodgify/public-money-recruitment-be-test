namespace Models.ViewModels.Calendar;

public sealed class CalendarViewModel
{
    public CalendarViewModel(int rentalId, List<CalendarDateViewModel> dates)
    {
        RentalId = rentalId;
        Dates = dates;
    }

    public int RentalId { get; set; }
    public List<CalendarDateViewModel> Dates { get; set; }
}
