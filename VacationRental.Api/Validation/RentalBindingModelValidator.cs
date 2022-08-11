using FluentValidation;
using VacationRental.Api.Contracts.Request;

namespace VacationRental.Api.Validation
{
    public class RentalBindingModelValidator : AbstractValidator<RentalBindingModel>
    {
        public RentalBindingModelValidator()
        {
            RuleFor(dto => dto.Units).NotNull().WithMessage("unit must not empty");

            RuleFor(dto => dto.PreparationTimeInDays)
                .NotNull()
                .WithMessage("PreparationTimeInDays must not empty")
                .GreaterThanOrEqualTo(0)
                .WithMessage("PreparationTimeInDays must be greater than or equal to 0");
        }
    }
}
