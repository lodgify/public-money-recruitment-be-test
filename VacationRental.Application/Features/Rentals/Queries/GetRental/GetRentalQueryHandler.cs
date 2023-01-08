using AutoMapper;
using System;
using VacationRental.Application.Contracts.Persistence;
using VacationRental.Application.Contracts.Pipeline;
using VacationRental.Application.Exceptions;
using VacationRental.Domain.Errors;
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

        public RentalDto Handle(GetRentalQuery request)
        {            
            var rental = _rentalRepository.GetById(request.RentalId);
            if (rental == null)
                throw new NotFoundException(RentalError.RentalNotFound);

            return _mapper.Map<RentalDto>(rental);
        }
    }
}
