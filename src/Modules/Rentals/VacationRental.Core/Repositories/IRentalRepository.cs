using VacationRental.Core.Entities;

namespace VacationRental.Core.Repositories
{
    internal interface IRentalRepository
    {
        Rental Get(int id);
        int Add(Rental rental);
    }
}
