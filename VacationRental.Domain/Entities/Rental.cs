namespace VacationRental.Domain.Entities
{
    public class Rental
    {
        public int Id { get; set; }
        public int PreparationTimeInDays { get; set; }
        public List<Unit> Units { get; set; }
    }
}