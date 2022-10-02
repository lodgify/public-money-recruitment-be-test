namespace VacationRental.Domain.VacationRental.Models
{
    public class RentalViewModel
    {
        /// <summary>
        /// Rental identification ID
        /// </summary>
        public int Id { get; set; }
        /// <summary>
        /// Number of Units for each rental
        /// </summary>
        public int Units { get; set; }
        /// <summary>
        /// Number of days to prepare the unit to the next booking
        /// </summary>
        public int PreparationTimeInDays { get; set; }
    }
}
