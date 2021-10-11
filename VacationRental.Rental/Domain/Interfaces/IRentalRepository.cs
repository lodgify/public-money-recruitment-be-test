using VacationRental.Domain.Interfaces;

namespace VacationRental.Rental.Domain.Interfaces
{
    public interface IRentalRepository : IAsyncRepository<Rental>
    {
    }
}