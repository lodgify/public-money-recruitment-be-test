using System;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using FluentValidation;
using VacationRental.Api.Contracts.Request;
using VacationRental.Api.Contracts.Response;
using VacationRental.Api.Interfaces;
using VacationRental.Api.Models;
using VacationRental.Api.Repository;

namespace VacationRental.Api.Services
{
    public class RentalService : IRentalService
    {
        private readonly IRentalRepository _rentalRepository;
        private readonly IMapper _mapper;
        private readonly IValidator<RentalBindingModel> _validator;
        private readonly IValidator<UpdateRentalBindingModel> _updateValidator;

        public RentalService(
            IRentalRepository rentalRepository,
            IMapper mapper,
            IValidator<RentalBindingModel> validator,
            IValidator<UpdateRentalBindingModel> updateValidator
        )
        {
            _rentalRepository = rentalRepository;
            _mapper = mapper;
            _validator = validator;
            _updateValidator = updateValidator;
        }

        public RentalViewModel GetRental(int id)
        {
            var rental = _rentalRepository.Get(id);

            if (rental is null)
                throw new ApplicationException("Rental not found");

            return rental;
        }

        public async Task<ResourceIdViewModel> CreateAsync(RentalBindingModel model)
        {
            var validation = await _validator.ValidateAsync(model);

            if (!validation.IsValid)
                throw new ApplicationException(validation.Errors.First().ErrorMessage);

            var newRental = _mapper.Map<RentalViewModel>(
                model,
                opt => opt.Items["Id"] = _rentalRepository.RentalsCount() + 1
            );

            var id = _rentalRepository.Create(newRental);

            return new ResourceIdViewModel { Id = id };
        }

        public async Task UpdateAsync(UpdateRentalBindingModel model)
        {
            var validation = await _updateValidator.ValidateAsync(model);

            if (!validation.IsValid)
                throw new ApplicationException(validation.Errors.First().ErrorMessage);

            var rental = _rentalRepository.Get(model.Id);
            
            _mapper.Map(model, rental);

            _rentalRepository.Update(rental);
        }
    }
}
