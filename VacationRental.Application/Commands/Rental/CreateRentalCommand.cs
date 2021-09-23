using System;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using VacationRental.Domain.Repositories;

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


        public async Task Handle(CreateRentalCommand request)
        {
            
        }
    }
}
