namespace VacationRental.Domain.Exceptions
{
    public abstract class EntityNotFoundException : DomainException
    {
        protected EntityNotFoundException(string message) : base(message) { }
    }
}
