namespace VacationRental.Domain.VacationRental.Models
{
    public class BookingViewModel
    {
        /// <summary>
        /// Booking identification ID
        /// </summary>
        public int Id { get; set; }
        /// <summary>
        /// Rental identification ID
        /// </summary>
        public int RentalId { get; set; }
        /// <summary>
        /// Start booking date
        /// </summary>
        public DateTime Start { get; set; }
        /// <summary>
        /// End booking date
        /// </summary>
        public DateTime End { get; set; }
        /// <summary>
        /// Number of nights booked
        /// </summary>
        public int Nights { get; set; }
        /// <summary>
        /// Number of the unit booked
        /// </summary>
        public int Unit { get; set; }
    }
}
