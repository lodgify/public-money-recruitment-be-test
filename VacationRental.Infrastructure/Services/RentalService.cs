using Mapster;
using VacationRental.Data;
using VacationRental.Domain.Rentals;
using VacationRental.Infrastructure.DTOs;
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

        public Rental UpdateRental(RentalUpdateInputDTO model, int id)
        {
            try
            {
                var rentalToUpdate = _renatalsRepository.GetEntityById(id);

                rentalToUpdate.Update(model.Adapt<Rental>());

                _renatalsRepository.Update(rentalToUpdate);

                return rentalToUpdate;
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}
