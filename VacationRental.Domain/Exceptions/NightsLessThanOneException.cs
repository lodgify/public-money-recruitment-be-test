namespace VacationRental.Domain.Exceptions
{
    public sealed class NightsLessThanOneException : DomainException
    {
        public NightsLessThanOneException() : base("The number of nights should be equal to or greater than one")
        {
            
        }
    }
}
