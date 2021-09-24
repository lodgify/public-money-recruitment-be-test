namespace VacationRental.Domain.Exceptions
{
    public class EntityNotFoundException : DomainException
    {
        protected EntityNotFoundException(string message) : base(message) { }
    }
}
