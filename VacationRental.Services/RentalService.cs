using AutoMapper;
using Microsoft.Extensions.Logging;
using VacationRental.DataAccess.Interfaces;
using VacationRental.DataAccess.Models.Entities;
using VacationRental.Models.Dtos;
using VacationRental.Models.Exceptions;
using VacationRental.Models.Paramaters;
using VacationRental.Services.Interfaces;

namespace VacationRental.Services
{
    public class RentalService : IRentalService
    {
        private readonly IGenericRepository<Rental> _rentalRepository;

        private readonly IMapper _mapper;
        private readonly ILogger<RentalService> _logger;

        public RentalService(IGenericRepository<Rental> rentalRepository, IMapper mapper, ILogger<RentalService> logger) 
        {
            _rentalRepository = rentalRepository;

            _mapper = mapper;
            _logger = logger;
        }

        public async Task<IEnumerable<RentalDto>> GetRentalsAsync()
        {
            _logger.LogInformation($"{nameof(GetRentalsAsync)}.");

            var rentals = await _rentalRepository.FindAsync(x => x.IsActive);

            var result = _mapper.Map<IEnumerable<RentalDto>>(rentals);

            _logger.LogInformation($"Rentals was getting successfully.");

            return result;
        }

        public async Task<RentalDto> GetRentalByIdAsync(int rentalId)
        {
            _logger.LogInformation($"{nameof(GetRentalByIdAsync)} with params: '{nameof(rentalId)}'={rentalId}.");

            var rental = await _rentalRepository.GetByIdAsync(rentalId);
            if (rental == null)
            {
                var message = $"{nameof(Rental)} with Id: {rentalId} not found.";

                _logger.LogError(message);
                throw new RentalNotFoundException(message);
            }

            var result = _mapper.Map<RentalDto>(rental);

            _logger.LogInformation($"{nameof(Rental)} with Id: {rentalId} was getting successfully.");

            return result;
        }

        public async Task<BaseEntityDto> AddRentalAsync(RentalParameters parameters)
        {
            _logger.LogInformation($"{nameof(AddRentalAsync)} with params: '{nameof(parameters.Units)}'={parameters.Units}, " +
                                                                         $"'{nameof(parameters.PreparationTimeInDays)}'={parameters.PreparationTimeInDays}.");

            var rental = _mapper.Map<Rental>(parameters);

            await _rentalRepository.AddAsync(rental);

            _logger.LogInformation($"{nameof(Rental)} with Id: {rental.Id} was created successfully.");

            var result = _mapper.Map<RentalDto>(rental);

            return result;
        }

        public async Task UpdateRentalAsync(int rentalId, RentalParameters parameters)
        {
            _logger.LogInformation($"{nameof(UpdateRentalAsync)} with Id: {rentalId} and params: '{nameof(parameters.Units)}'={parameters.Units}, " +
                                                                                               $"'{nameof(parameters.PreparationTimeInDays)}'={parameters.PreparationTimeInDays}.");

            var entity = await _rentalRepository.GetByIdAsync(rentalId);
            if (entity == null)
            {
                var message = $"{nameof(Rental)} with Id: {rentalId} not found.";

                _logger.LogError(message);
                throw new RentalNotFoundException(message);
            }

            // TODO: 

            var rental = _mapper.Map<Rental>(parameters);

            entity.Modified = DateTime.UtcNow;
            entity.PreparationTimeInDays = rental.PreparationTimeInDays;
            entity.Units = rental.Units;
            entity.IsActive = true;

            await _rentalRepository.UpdateAsync(rental);

            _logger.LogInformation($"{nameof(Rental)} with Id: {rental.Id} was created successfully.");
        }

        public async Task DeleteRentalAsync(int rentalId)
        {
            _logger.LogInformation($"{nameof(DeleteRentalAsync)} with Id: {rentalId}.");

            var rental = await _rentalRepository.GetByIdAsync(rentalId);
            if (rental == null)
            {
                var message = $"{nameof(Rental)} with Id: {rentalId} not found.";

                _logger.LogError(message);
                throw new RentalNotFoundException(message);
            }

            rental.IsActive = false;

            await _rentalRepository.UpdateAsync(rental);

            _logger.LogInformation($"{nameof(Rental)} with Id: {rental.Id} was deleted successfully.");
        }
    }
}
