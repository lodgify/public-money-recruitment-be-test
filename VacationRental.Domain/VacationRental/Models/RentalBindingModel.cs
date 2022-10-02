namespace VacationRental.Domain.VacationRental.Models
{
    public class RentalBindingModel
    {
        /// <summary>
        /// Number of Units for each rental
        /// </summary>
        public int Units { get; set; }
        /// <summary>
        /// Number of days for any booking without another booking after, an extra day is blocked for the unit where the existing booking occurs.
        /// </summary>
        public int PreparationTimeInDays { get; set; }
    }
}
