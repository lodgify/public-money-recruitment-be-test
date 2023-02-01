using FluentValidation;
using VacationRental.Api.Models;

namespace VacationRental.Api.Validators;

public sealed class VacationRentalBindingModelValidator : AbstractValidator<VacationRentalBindingModel>
{
    public VacationRentalBindingModelValidator()
    {
        RuleFor(x => x.PreparationTimeInDays)
            .GreaterThanOrEqualTo(1)
            .LessThanOrEqualTo(3);
    }
}