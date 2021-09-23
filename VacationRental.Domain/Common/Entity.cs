namespace VacationRental.Domain.Common
{
    public class Entity<TIdentifier>
    {
        public TIdentifier Id { get; } // Identifier can't be changed.
    }
}
