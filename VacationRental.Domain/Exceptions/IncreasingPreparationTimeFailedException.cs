namespace VacationRental.Domain.Exceptions
{
    public class IncreasingPreparationTimeFailedException : DomainException
    {
        public IncreasingPreparationTimeFailedException() : base("Increasing the preparation period causes overlapping")
        {
            
        }
    }
}
