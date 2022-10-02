namespace VacationRental.Domain.VacationRental.Models
{
    public class BookingBindingModel
    {
        /// <summary>
        /// Rental identification ID
        /// </summary>
        public int RentalId { get; set; }

        /// <summary>
        /// Start date of Booking wished
        /// </summary>
        public DateTime Start
        {
            get => _startIgnoreTime;
            set => _startIgnoreTime = value.Date;
        }

        /// <summary>
        /// Ignore time to dates
        /// </summary>
        private DateTime _startIgnoreTime;

        /// <summary>
        /// Number of nights wished to booking
        /// </summary>
        public int Nights { get; set; }
    }
}
