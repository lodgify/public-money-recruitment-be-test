namespace VacationRental.Core.Entities
{
    internal class Rental
    {
        public int Id { get; private set; }
        public int Units { get; private set; }

        public Rental(int id, int units)
        {
            Id = id;
            Units = units;
        }
    }
}
