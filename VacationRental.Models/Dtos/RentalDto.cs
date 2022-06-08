namespace VacationRental.Models.Dtos
{
    public class RentalDto : BaseEntityDto
    {
        public int Units { get; set; }
        public int PreparationTimeInDays { get; set; }
    }
}
