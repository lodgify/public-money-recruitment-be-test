namespace VacationRental.Domain.Exceptions
{
    public class DecreasingNumberOfUnitsFailedException : DomainException
    {
        public DecreasingNumberOfUnitsFailedException() : base("Decreasing the number of units causes overlapping")
        {
            
        }
    }
}
