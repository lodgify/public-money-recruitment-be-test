using System.Threading;
using System.Threading.Tasks;
using VacationRental.Core.Entities;
using VacationRental.Core.Repositories;
using VacationRental.Shared.Abstractions.Commands;

namespace VacationRental.Application.Commands.Handlers
{
    internal class AddRentalHandler : ICommandHandler<AddRental, int>
    {
        private readonly IRentalRepository _rentalRepository;

        public AddRentalHandler(IRentalRepository rentalRepository)
        {
            _rentalRepository = rentalRepository;
        }

        public Task<int> HandleAsync(AddRental command, CancellationToken cancellationToken = default)
        {
            var rental = new Rental(command.Units);
            var rentalId = _rentalRepository.Add(rental);

            return Task.FromResult(rentalId);
        }
    }
}
