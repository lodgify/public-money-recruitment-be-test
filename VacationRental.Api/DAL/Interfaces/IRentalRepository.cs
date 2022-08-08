using VacationRental.Api.Models;

namespace VacationRental.Api.DAL.Interfaces
{
    public interface IRentalRepository : IRepository<RentalViewModel>
    {
        void Update(int id, RentalViewModel model);
    }
}
