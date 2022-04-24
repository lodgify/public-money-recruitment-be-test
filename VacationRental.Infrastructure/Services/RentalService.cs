using VacationRental.Data;
using VacationRental.Domain.Rentals;
using VacationRental.Infrastructure.Services.Interfaces;

namespace VacationRental.Infrastructure.Services
{
    public class RentalService : IRentalService
    {
        private readonly IEntityRepository<Rental> _renatalsRepository;

        public RentalService(IEntityRepository<Rental> renatalsRepository)
        {
            _renatalsRepository = renatalsRepository;
        }

        public int CreateRental(Rental rental)
        {
            try
            {
                var addedRental = _renatalsRepository.Add(rental);

                return addedRental.Id;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public Rental GetRental(int id)
        {
            try
            {
                return _renatalsRepository.GetEntityById(id);
            }
            catch (Exception)
            {
                throw;
            }
        }

        public void UpdateRental(Rental rental, int id)
        {
            throw new NotImplementedException();
        }
    }
}
