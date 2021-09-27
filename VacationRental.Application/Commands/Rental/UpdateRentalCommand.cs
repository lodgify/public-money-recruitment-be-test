using System;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.Extensions.Logging;
using VacationRental.Domain.Repositories;
using VacationRental.Domain.Services;
using VacationRental.Domain.Values;

namespace VacationRental.Application.Commands.Rental
{
    public class UpdateRentalCommand : IRequestHandler<UpdateRentalRequest>
    {
        private readonly IRentalRepository _rentalRepository;
        private readonly IRentalUpdatedEventHandler _rentalUpdatedHandler;
        private readonly ILogger<UpdateRentalRequest> _logger;

        public UpdateRentalCommand(IRentalRepository rentalRepository, IRentalUpdatedEventHandler rentalUpdatedHandler, ILogger<UpdateRentalRequest> logger)
        {
            _rentalRepository = rentalRepository ?? throw new ArgumentNullException(nameof(rentalRepository));
            _rentalUpdatedHandler = rentalUpdatedHandler ?? throw new ArgumentNullException(nameof(rentalUpdatedHandler));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        public async Task<Unit> Handle(UpdateRentalRequest request, CancellationToken cancellationToken)
        {
            var rentalId = new RentalId(request.Id);
            var rental = await _rentalRepository.Get(rentalId);
            rental.RentalUpdateEvent += _rentalUpdatedHandler.Handle;

            await rental.Update(request.Units, request.PreparationTimeInDays);

            await _rentalRepository.Update(rental);
            _logger.LogInformation($"Rental '{rentalId}' is updated successfully");

            return Unit.Value;
        }
    }
}
