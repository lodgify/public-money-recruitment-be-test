namespace VacationRental.Domain.Common
{
    public abstract class Entity<TIdentifier>
    {
        public TIdentifier Id { get; } // Identifier can't be changed.

        protected Entity(TIdentifier id)
        {
            Id = id;
        }
    }
}
