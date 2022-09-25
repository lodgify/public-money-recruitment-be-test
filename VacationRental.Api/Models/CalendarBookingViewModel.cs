namespace VacationRental.Api.Models
{
    public class CalendarBookingViewModel
    {
        public int Id { get; set; }
        // isCleaning will give the calendar more information on the reason of the room being occupied
        public bool isCleaning { get; set; }
    }
}