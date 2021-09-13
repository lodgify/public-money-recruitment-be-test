namespace VacationRental.Application
{
    public interface ICalendarService 
    {
        GetCalendarResponse GetBooking(GetCalendarRequest request);
    }
}
