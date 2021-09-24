using VacationRental.Domain.Values;

namespace VacationRental.Domain.Exceptions
{
    public sealed class RentalNotFoundException : EntityNotFoundException
    {
        public RentalId Id { get; }
        public RentalNotFoundException(RentalId rentalId) : base($"{rentalId.Id} isn't found")
        {
            
        }
    }
}
