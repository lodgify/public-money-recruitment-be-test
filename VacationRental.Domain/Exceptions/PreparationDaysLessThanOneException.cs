namespace VacationRental.Domain.Exceptions
{
    public sealed class PreparationDaysLessThanOneException : DomainException
    {
        public PreparationDaysLessThanOneException() : base("The number of preparation days should be equal to or greater than one")
        {
            
        }
    }
}
