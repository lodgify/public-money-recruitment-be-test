using System;
using System.Threading.Tasks;
using VacationRental.Domain.Repositories;
using VacationRental.Domain.Values;

namespace VacationRental.Application.Queries.Rental
{
    public class RentalByIdQueryHandler
    {
        private readonly IRentalRepository _rentalRepository;

        public RentalByIdQueryHandler(IRentalRepository rentalRepository)
        {
            _rentalRepository = rentalRepository ?? throw new ArgumentNullException(nameof(rentalRepository));
        }

        public async Task<RentalViewModel> Handle(GetRentalById query)
        {
            var domainModel = _rentalRepository.Get(new RentalId(query.Id));
            return null;
        }
    }
}
