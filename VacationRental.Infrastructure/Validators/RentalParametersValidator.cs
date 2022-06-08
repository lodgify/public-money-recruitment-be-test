using FluentValidation;
using VacationRental.Models.Paramaters;

namespace VacationRental.Infrastructure.Validators
{
    public class RentalParametersValidator : AbstractValidator<RentalParameters>
    {
        public RentalParametersValidator()
        {
            RuleFor(x => x.Units).NotNull().WithMessage(configure => $"{nameof(configure.Units)} is required.");
            RuleFor(x => x.Units).NotNull().Must(x => x.HasValue && x.Value > 0).WithMessage(configure => $"{nameof(configure.Units)} must be positive and grater than 0.");

            RuleFor(x => x.PreparationTimeInDays).NotNull().WithMessage(configure => $"{nameof(configure.PreparationTimeInDays)} is required.");
            RuleFor(x => x.PreparationTimeInDays).NotNull().Must(x => x.HasValue && x.Value > 0).WithMessage(configure => $"{nameof(configure.PreparationTimeInDays)} must be positive and grater than 0.");
        }
    }
}
