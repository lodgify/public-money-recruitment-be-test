namespace VacationRental.Domain.Exceptions
{
    public sealed class EntityIdentifierIsNullException : DomainException
    {
        public EntityIdentifierIsNullException() : base("Identifier can't be null")
        {
            
        }
    }
}
