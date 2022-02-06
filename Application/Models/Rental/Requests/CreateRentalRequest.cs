namespace VacationRental.Api.Models
{
    public class CreateRentalRequest
    {
        public int Units { get; set; }
        
        public int PreparationTimeInDays { get; set; }
    }
}
