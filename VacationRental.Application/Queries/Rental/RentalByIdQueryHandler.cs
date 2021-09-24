using System;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using VacationRental.Domain.Repositories;
using VacationRental.Domain.Values;

namespace VacationRental.Application.Queries.Rental
{
    public class RentalByIdQueryHandler : IRequestHandler<GetRentalByIdQuery, RentalViewModel>
    {
        private readonly IRentalRepository _rentalRepository;

        public RentalByIdQueryHandler(IRentalRepository rentalRepository)
        {
            _rentalRepository = rentalRepository ?? throw new ArgumentNullException(nameof(rentalRepository));
        }

        public async Task<RentalViewModel> Handle(GetRentalByIdQuery query, CancellationToken cancellationToken)
        {
            var domainModel = _rentalRepository.Get(new RentalId(query.Id));
            return null;
        }
    }
}
