namespace VacationRental.Domain.VacationRental.Models
{
    public class CalendarDateViewModel
    {
        /// <summary>
        /// Start booking date
        /// </summary>
        public DateTime Date { get; set; }
        /// <summary>
        /// List of bookings
        /// </summary>
        public List<CalendarBookingViewModel> Bookings { get; set; }
        /// <summary>
        /// List of days to prepare the unit to the next booking
        /// </summary>
        public List<Units> PreparationTimes { get; set; }

        public class Units
        {
            /// <summary>
            /// Number of Units for each rental
            /// </summary>
            public int Unit { get; set; }
        }
    }
}
