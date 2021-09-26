using VacationRental.Domain.Exceptions;

namespace VacationRental.Domain.Common
{
    public abstract class Entity<TIdentifier>
    where TIdentifier : ValueObject
    {
        public TIdentifier Id { get; } // Identifier can't be changed.

        protected Entity(TIdentifier id)
        {
            Id = id ?? throw new EntityIdentifierIsNullException();
        }
    }
}
