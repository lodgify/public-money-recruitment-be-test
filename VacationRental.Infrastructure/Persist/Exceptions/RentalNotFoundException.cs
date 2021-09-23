using VacationRental.Application.Exceptions;
using VacationRental.Domain.Values;

namespace VacationRental.Infrastructure.Persist.Exceptions
{
    public sealed class RentalNotFoundException : InfrastructureException
    {
        public RentalId Id { get; }
        public RentalNotFoundException(RentalId rentalId) : base($"{rentalId.Id} isn't found")
        {
            
        }
    }
}
