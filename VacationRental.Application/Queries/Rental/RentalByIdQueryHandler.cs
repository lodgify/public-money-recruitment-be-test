using System;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using VacationRental.Domain.Repositories.ReadOnly;
using VacationRental.Domain.Values;

namespace VacationRental.Application.Queries.Rental
{
    public class RentalByIdQueryHandler : IRequestHandler<GetRentalByIdQuery, RentalViewModel>
    {
        private readonly IRentalReadOnlyRepository _rentalRepository;

        public RentalByIdQueryHandler(IRentalReadOnlyRepository rentalRepository)
        {
            _rentalRepository = rentalRepository ?? throw new ArgumentNullException(nameof(rentalRepository));
        }

        public async Task<RentalViewModel> Handle(GetRentalByIdQuery query, CancellationToken cancellationToken)
        {
            var domainModel = await _rentalRepository.Get(new RentalId(query.Id));

            return new RentalViewModel
            {
                Id = (int) domainModel.Id,
                Units = domainModel.Units,
                PreparationTimeIdDays = domainModel.PreparationTimeInDays
            };
        }
    }
}
