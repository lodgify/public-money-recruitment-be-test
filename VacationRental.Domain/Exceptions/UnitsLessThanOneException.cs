namespace VacationRental.Domain.Exceptions
{
    public class UnitsLessThanOneException : DomainException
    {
        public UnitsLessThanOneException() : base("The number of units should be equal to or greater than one")
        {
            
        }
    }
}
