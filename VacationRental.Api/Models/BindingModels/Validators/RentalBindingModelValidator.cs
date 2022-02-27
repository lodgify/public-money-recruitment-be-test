using FluentValidation;

namespace VacationRental.Api.Models
{
    public class RentalBindingModelValidator : AbstractValidator<RentalBindingModel>
    {
        public RentalBindingModelValidator()
        {
            CascadeMode = CascadeMode.Stop;

            RuleFor(x => x.Units).GreaterThan(0);

            RuleFor(x => x.PreparationTimeInDays).GreaterThan(0);
        }
    }
}
