using System;
using VacationRental.Api.DAL.Interfaces;
using VacationRental.Api.Models;

namespace VacationRental.Api.Services
{
    public class RentalService : IRentalService
    {
        private readonly IRentalRepository _rentalRepository;

        public RentalService(IRentalRepository rentalRepository)
        {
            _rentalRepository = rentalRepository;
        }
        public ResourceIdViewModel Create(RentalBindingModel model)
        {
            var key = new ResourceIdViewModel { Id = _rentalRepository.Count + 1 };

            _rentalRepository.Add(key.Id, new RentalViewModel
            {
                Id = key.Id,
                Units = model.Units,
                PreparationTimeInDays = model.PreparationTimeInDays,
            });

            return key;
        }

        public RentalViewModel Get(int id)
        {
            if (!_rentalRepository.HasValue(id))
                throw new ApplicationException("Rental not found");

            return _rentalRepository.Get(id);
        }

        public void Update(int id, RentalBindingModel model)
        {
            if (!_rentalRepository.HasValue(id))
                throw new ApplicationException("Rental not found");

            var rental = _rentalRepository.Get(id);

            // TODO check is crossing will appear if we change it

            rental.Units = model.Units;
            rental.PreparationTimeInDays = model.PreparationTimeInDays;
        }
    }
}
