using System;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using VacationRental.Domain.Repositories;
using VacationRental.Domain.Values;

namespace VacationRental.Application.Commands.Rental
{
    public class CreateRentalCommand
    {

        private readonly IRentalRepository _rentalRepository;
        private readonly ILogger<CreateRentalCommand> _logger;

        public CreateRentalCommand(IRentalRepository rentalRepository,  ILogger<CreateRentalCommand> logger)
        {
            _rentalRepository = rentalRepository ?? throw new ArgumentNullException(nameof(rentalRepository));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }


        public async Task Handle(RentalBindingModel request)
        {
            var newRental = _rentalRepository.Add(new Domain.Entities.Rental(RentalId.Empty, request.Units,
                request.PreparationTimeInDays));

            _logger.LogInformation($"Rental '{newRental.Id}' has been created successfully");
        }
    }
}
