namespace VacationRental.Core.Entities
{
    internal class Rental
    {
        public int Id { get; private set; }
        public int Units { get; private set; }

        public Rental(int units)
        {
            Units = units;
        }

        public void SetRentalId(int id)
        {
            Id = id;
        }
    }
}
