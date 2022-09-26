using System;
using AutoMapper;
using VacationRental.BusinessLogic.Services.Interfaces;
using VacationRental.BusinessObjects;
using VacationRental.Repository.Entities;
using VacationRental.Repository.Repositories.Interfaces;

namespace VacationRental.BusinessLogic.Services
{
    public class RentalsService : IRentalsService
    {
        private readonly IRentalRepository _rentalRepository;
        private readonly IMapper _mapper;

        public RentalsService(IRentalRepository rentalRepository, 
            IMapper mapper)
        {
            _rentalRepository = rentalRepository;
            _mapper = mapper;
        }

        public Rental GetRental(int rentalId)
        {
            var rentalEntity = _rentalRepository.GetRentalEntity(rentalId);
            if (rentalEntity == null)
            {
                throw new ArgumentException($"Rental with id {rentalId} does not exist.");
            }

            var rental = _mapper.Map<Rental>(rentalEntity);

            return rental;
        }

        public int CreateRental(CreateRental createRental)
        {
            var rentalEntity = _mapper.Map<RentalEntity>(createRental);
            var rentalId = _rentalRepository.CreateRentalEntity(rentalEntity);

            return rentalId;
        }
    }
}
