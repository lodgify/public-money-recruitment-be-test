using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using VacationRental.Api.Contracts.Request;
using VacationRental.Api.Contracts.Response;
using VacationRental.Api.Models;
using VacationRental.Api.Repository;

namespace VacationRental.Api.Services
{
    public class RentalService : IRentalService
    {
        private readonly IRentalRepository _rentalRepository;
        private readonly IMapper _mapper;

        public RentalService(IRentalRepository rentalRepository, IMapper mapper)
        {
            _rentalRepository = rentalRepository;
            _mapper = mapper;
        }

        public RentalViewModel GetRental(int id)
        {
            var rental = _rentalRepository.GetRental(id);

            if (rental is null)
                throw new ApplicationException("Rental not found");

            return rental;
        }

        public ResourceIdViewModel Create(RentalBindingModel model)
        {
            var newRental =
                _mapper.Map<RentalViewModel>(model, opt => opt.Items["Id"] = _rentalRepository.RentalsCount() + 1);

            var id = _rentalRepository.CreateRental(newRental);

            return new ResourceIdViewModel {Id = id};
        }
    }
}