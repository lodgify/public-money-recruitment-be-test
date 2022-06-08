namespace VacationRental.DataAccess.Models.Entities
{
    public class Booking : BaseEntity
    {
        public int RentalId { get; set; }
        public DateTime Start { get; set; }
        public int Nights { get; set; }
    }
}
