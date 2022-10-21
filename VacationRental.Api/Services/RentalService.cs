using LanguageExt;
using Microsoft.Extensions.Logging;
using System;
using VacationRental.Api.Core.Interfaces;
using VacationRental.Api.Core.Models;
using VacationRental.Api.Interfaces;

namespace VacationRental.Api.Services
{
    public class RentalService : IRentalService
    {
        private readonly IRentalRepository _rentalRepository;
        private readonly ILogger<RentalService> _logger;
        public RentalService(IRentalRepository rentalRepository, ILogger<RentalService> logger)
        {
            _rentalRepository = rentalRepository;
            _logger = logger;
        }

        public Result<RentalViewModel> GetRentalById(int rentalId)
        {
            try
            {
                _logger.LogInformation($"Get data with value: {rentalId}");
                return _rentalRepository.GetRental(rentalId);
            }
            catch (ApplicationException exception)
            {
                _logger.LogError($"An error occurred for method {nameof(GetRentalById)} with error:\n\n{exception.Message}");
                return new Result<RentalViewModel>(exception);
            }
        }

        public Result<ResourceIdViewModel> AddNewRental(RentalBindingModel model)
        {
            try
            {
                _logger.LogInformation($"Insert data with value: {model}");
                var newRental = _rentalRepository.InsertNewRental(model);
                if (newRental.Id == 0)
                    return new Result<ResourceIdViewModel>(new ApplicationException("not acceptable"));

                return newRental;
            }
            catch (ApplicationException exception)
            {
                _logger.LogError($"An error occurred for method {nameof(AddNewRental)} with error:\n\n{exception.Message}");
                return new Result<ResourceIdViewModel>(exception);
            }
        }

        public Result<bool> UpdateRentalById(int rentalId, RentalBindingModel model)
        {
            try
            {
                _logger.LogInformation($"Update data Id: {rentalId} with values: {model}");
                var updated = _rentalRepository.UpdateRental(rentalId, model);
                if(!updated)
                    return new Result<bool>(new ApplicationException("not modified"));

                return updated;
            }
            catch (ApplicationException exception)
            {
                _logger.LogError($"An error occurred for method {nameof(UpdateRentalById)} with error:\n\n{exception.Message}");
                return new Result<bool>(exception);
            }
        }
    }
}
