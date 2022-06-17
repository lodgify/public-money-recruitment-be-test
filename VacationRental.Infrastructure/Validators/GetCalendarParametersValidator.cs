using FluentValidation;
using VacationRental.Models.Paramaters;

namespace VacationRental.Infrastructure.Validators
{
    public class GetCalendarParametersValidator : AbstractValidator<GetCalendarParameters>
    {
        public GetCalendarParametersValidator()
        {
            RuleFor(x => x.Nights).NotNull().WithMessage(configure => $"{nameof(configure.Nights)} is required.");
            RuleFor(x => x.Nights).NotNull().Must(x => x.HasValue && x.Value > 0).WithMessage(configure => $"{nameof(configure.Nights)} must be positive and grater than 0.");

            RuleFor(x => x.Start).NotNull().WithMessage(configure => $"{nameof(configure.Start)} is required.");
            RuleFor(x => x.RentalId).NotNull().WithMessage(configure => $"{nameof(configure.RentalId)} is required.");
        }
    }
}
