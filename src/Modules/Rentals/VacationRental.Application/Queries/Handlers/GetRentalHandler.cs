using System.Threading;
using System.Threading.Tasks;
using VacationRental.Application.DTO;
using VacationRental.Core.Exceptions;
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

        public async Task<RentalDto> HandleAsync(GetRental query, CancellationToken cancellationToken = default)
        {
            var rental = await _rentalRepository.GetAsync(query.Id, cancellationToken);

            if (rental is null)
            {
                throw new RentalNotExistException(query.Id);
            }

            return rental.AsDto();
        }
    }
}
