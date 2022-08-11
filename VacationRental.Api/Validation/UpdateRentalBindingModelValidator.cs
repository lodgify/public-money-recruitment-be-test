using FluentValidation;
using VacationRental.Api.Contracts.Request;
using VacationRental.Api.Repository;

namespace VacationRental.Api.Validation
{
    public class UpdateRentalBindingModelValidator : AbstractValidator<UpdateRentalBindingModel>
    {
        private readonly IRentalRepository _rentalRepository;

        public UpdateRentalBindingModelValidator(IRentalRepository rentalRepository)
        {
            _rentalRepository = rentalRepository;

            RuleFor(dto => dto.Units).NotNull().WithMessage("unit must not empty");

            RuleFor(dto => dto.PreparationTimeInDays)
                .NotNull()
                .WithMessage("PreparationTimeInDays must not empty")
                .GreaterThanOrEqualTo(0)
                .WithMessage("PreparationTimeInDays must be greater than or equal to 0");

            RuleFor(dto => dto.Id).Must(RentalExists).WithMessage("Rental not Found");
        }

        private bool RentalExists(int rentalId) => _rentalRepository.Get(rentalId) != null;
    }
}
