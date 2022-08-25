using System;
using AutoMapper;
using VacationRental.Core.Data;
using VacationRental.Core.Domain.Rentals;
using VacationRental.Services.Models.Rental;

namespace VacationRental.Services.Rentals
{
    public class RentalService : IRentalService

    {
        private readonly IRepository<RentalEntity, int> _rentalRepository;
        private readonly IConfigurationProvider _mappingConfiguration;

        public RentalService(
            IRepository<RentalEntity, int> rentalRepository, 
            IConfigurationProvider mappingConfiguration)
        {
            _rentalRepository = rentalRepository;
            _mappingConfiguration = mappingConfiguration;
        }

        public RentalViewModel Get(int rentalId)
        {
            var rentalEntity = _rentalRepository.GetById(rentalId);

            return _mappingConfiguration.CreateMapper().Map<RentalViewModel>(rentalEntity);
        }

        public RentalEntity Add(RentalBindingModel request)
        {
            if (request.Units <= 0)
                throw new ApplicationException("rental units must be greater than zero");

            if (request.PreparationTime < 0)
                throw new ApplicationException("preparation time can not be negative");

            var entity = new RentalEntity
            {
                Units = request.Units,
                PreparationTime = request.PreparationTime,
            };

            _rentalRepository.Insert(entity);

            return entity;
        }
    }
}
