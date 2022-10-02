namespace VacationRental.Domain.VacationRental.Models
{
    public class CalendarViewModel
    {
        /// <summary>
        /// Rental identification ID
        /// </summary>
        public int RentalId { get; set; }
        /// <summary>
        /// List of dates booked
        /// </summary>
        public List<CalendarDateViewModel> Dates { get; set; }
    }
}
