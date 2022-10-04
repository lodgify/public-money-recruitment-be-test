namespace VacationRental.Domain.Entities
{
    public class Booking
    {
        public int Id { get; set; }
        public DateTime Start { get; set; }
        public DateTime End { get; set; }
        public int UnitId { get; set; }
        public Unit Unit { get; set; }
    }
}