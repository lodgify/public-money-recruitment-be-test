using FluentValidation;
using VacationRental.Api.Models;

namespace VacationRental.Api.Validation.ValidationUnit
{
    public class RentalValidator : AbstractValidator<RentalBindingModel>
    {
        public RentalValidator()
        {
            this.RuleFor(request => request.Units).NotEmpty().NotNull().GreaterThan(0);
            this.RuleFor(request => request.PreparationTimeInDays).NotEmpty().NotNull().GreaterThan(0);
        }
    }
}
