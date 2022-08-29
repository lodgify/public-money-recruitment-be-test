using System;
using System.Collections.Generic;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using Microsoft.Extensions.Logging;
using VacationRental.Core.Data;
using VacationRental.Core.Domain.Rentals;
using VacationRental.Services.Exceptions;
using VacationRental.Services.Models.Rental;

namespace VacationRental.Services.Rentals
{
    public class RentalService : IRentalService

    {
        private readonly IRepository<RentalEntity, int> _rentalRepository;
        private readonly ILogger<RentalService> _logger;
        private readonly IMapper _mapper;

        public RentalService(
            IRepository<RentalEntity, int> rentalRepository,
            ILogger<RentalService> logger,
            IMapper mapper)
        {
            _rentalRepository = rentalRepository;
            _logger = logger;
            _mapper = mapper;
        }

        public IEnumerable<RentalDto> GetRentals()
        {
            var query = _rentalRepository
                .Table;

            return _mapper.ProjectTo<RentalDto>(query);
        }

        public RentalDto GetRentalBy(int rentalId)
        {
            var rentalEntity = _rentalRepository.GetById(rentalId);
            if (rentalEntity == null)
            {
                throw new RentalNotFoundException("Not found");
            }

            return _mapper.Map<RentalDto>(rentalEntity);
        }

        public RentalDto AddRental(CreateRentalRequest request)
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

            return _mapper.Map<RentalDto>(entity);
        }

        public RentalDto UpdateRental(int rentalId, CreateRentalRequest request)
        {
            if (request.Units <= 0)
                throw new ApplicationException("rental units must be greater than zero");

            if (request.PreparationTime < 0)
                throw new ApplicationException("preparation time can not be negative");

            var entity = _rentalRepository.GetById(rentalId);

            entity.PreparationTime = request.PreparationTime;
            entity.Units = request.Units;

            _rentalRepository.Update(entity);

            return _mapper.Map<RentalDto>(entity);
        }

        public bool DeleteRental(int rentalId)
        {
            var rental = _rentalRepository.GetById(rentalId);
            if (rental == null)
            {
                throw new RentalNotFoundException("Not found");
            }

            try
            {
                _rentalRepository.Delete(rental);
            }
            catch (Exception e)
            {
                _logger.LogError(e, $"An error occurred deleting rentalId: {rentalId}.");

                return false;
            }

            return true;
        }
    }
}
