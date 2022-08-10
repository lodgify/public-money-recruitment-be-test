using FluentValidation;
using VacationRental.Api.Contracts.Request;

namespace VacationRental.Api.Validation
{
    public class RentalBindingModelValidator : AbstractValidator<RentalBindingModel>
    {
        public RentalBindingModelValidator()
        {
            RuleFor(dto => dto.Units).NotNull().WithMessage("unit must not empty");
        }
    }
}