using System.Threading;
using System.Threading.Tasks;
using VacationRental.Core.Exceptions;
using VacationRental.Core.Repositories;
using VacationRental.Shared.Abstractions.Commands;

namespace VacationRental.Application.Commands.Handlers
{
    internal class UpdateRentalHandler : ICommandHandler<UpdateRental, int>
    {
        private readonly IRentalRepository _rentalRepository;

        public UpdateRentalHandler(IRentalRepository rentalRepository)
        {
            _rentalRepository = rentalRepository;
        }

        public async Task<int> HandleAsync(UpdateRental command, CancellationToken cancellationToken = default)
        {
            var rental = await _rentalRepository.GetAsync(command.RentalId, cancellationToken);

            if (rental is null)
            {
                throw new RentalNotExistException(command.RentalId);
            }

            rental.Update(command.Units, command.PreparationTimeInDays);

            await _rentalRepository.UpdateAsync(rental, cancellationToken);

            return rental.Id;
        }
    }
}
