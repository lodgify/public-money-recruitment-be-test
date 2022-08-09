using FluentValidation;
using VacationRental.Api.Contracts.Request;
using VacationRental.Api.Models;
using VacationRental.Api.Repository;

namespace VacationRental.Api.Validation
{
    public class BookingBindingModelValidator : AbstractValidator<BookingBindingModel>
    {
        private readonly IRentalRepository _rentalRepository;

        public BookingBindingModelValidator(IRentalRepository rentalRepository)
        {
            _rentalRepository = rentalRepository;

            RuleFor(dto => dto.Nights)
                .Cascade(CascadeMode.Stop)
                .GreaterThan(0)
                .WithMessage("Nights must be positive");
            
            RuleFor(dto=>dto.RentalId)
                .Cascade(CascadeMode.Stop)
                .Must(CheckRentalExists)
                .WithMessage("Rental not Found");
            
            
            
            
        }

        private bool CheckRentalExists(int rentalId)
            => _rentalRepository.GetRental(rentalId) != null;
        
        
    }
}