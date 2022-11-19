using System.Threading;
using System.Threading.Tasks;
using VacationRental.Application.DTO;
using VacationRental.Application.Exceptions;
using VacationRental.Core.Repositories;
using VacationRental.Shared.Abstractions.Queries;

namespace VacationRental.Application.Queries.Handlers
{
    internal class GetRentalHandler : IQueryHandler<GetRental, RentalDto>
    {
        private readonly IRentalRepository _rentalRepository;

        public GetRentalHandler(IRentalRepository rentalRepository)
        {
            _rentalRepository = rentalRepository;
        }

        public Task<RentalDto> HandleAsync(GetRental query, CancellationToken cancellationToken = default)
        {
            var rental = _rentalRepository.Get(query.Id);

            if (rental is null)
            {
                throw new RentalNotFoundException(query.Id);
            }

            return Task.FromResult(rental.AsDto());
        }
    }
}
