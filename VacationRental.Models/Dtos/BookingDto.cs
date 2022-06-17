namespace VacationRental.Models.Dtos
{
    public class BookingDto : BaseEntityDto
    {
        public int RentalId { get; set; }
        public DateTime Start { get; set; }
        public int Nights { get; set; }
    }
}
