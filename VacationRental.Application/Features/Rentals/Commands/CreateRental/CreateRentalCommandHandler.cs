using System.Threading;
using System.Threading.Tasks;
using VacationRental.Application.Contracts.Mediatr;
using VacationRental.Application.Contracts.Persistence;
using VacationRental.Domain.Entities;
using VacationRental.Domain.Models.Rentals;

namespace VacationRental.Application.Features.Rentals.Commands.CreateRental
{
    public class CreateRentalCommandHandler : ICommandHandler<CreateRentalCommand, ResourceId>
    {
        private readonly IRepository<Rental> _rentalRepository;

        public CreateRentalCommandHandler(IRepository<Rental> rentalRepository)
        {
            _rentalRepository = rentalRepository;
        }

        public Task<ResourceId> Handle(CreateRentalCommand request, CancellationToken cancellationToken)
        {
            var rental = _rentalRepository.Add(Rental.Create(request.Units, request.PreparationTimeInDays));

            return Task.FromResult(new ResourceId()
            {
                Id = rental.Id
            });
        }
    }
}
