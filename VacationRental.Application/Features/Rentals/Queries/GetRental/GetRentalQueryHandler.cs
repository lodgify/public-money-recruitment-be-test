using AutoMapper;
using System;
using System.Threading;
using System.Threading.Tasks;
using VacationRental.Application.Contracts.Mediatr;
using VacationRental.Application.Contracts.Persistence;
using VacationRental.Domain.Messages.Rentals;
using VacationRental.Domain.Models.Rentals;

namespace VacationRental.Application.Features.Rentals.Queries.GetRental
{
    public class GetRentalQueryHandler : IQueryHandler<GetRentalQuery, RentalDto>
    {
        private readonly IRepository<Rental> _rentalRepository;
        private readonly IMapper _mapper;

        public GetRentalQueryHandler(IRepository<Rental> rentalRepository, IMapper mapper)
        {
            _rentalRepository = rentalRepository;
            _mapper = mapper;
        }

        public Task<RentalDto> Handle(GetRentalQuery request, CancellationToken cancellationToken)
        {
            var rental = _rentalRepository.GetById(request.RentalId);
            if (rental == null)
                throw new ApplicationException("Rental not found");

            return Task.FromResult(_mapper.Map<RentalDto>(rental));
        }
    }
}
