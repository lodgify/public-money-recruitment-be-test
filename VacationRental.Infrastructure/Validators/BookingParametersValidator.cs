using FluentValidation;
using VacationRental.Models.Paramaters;

namespace VacationRental.Infrastructure.Validators
{
    public class BookingParametersValidator : AbstractValidator<BookingParameters>
    {
        public BookingParametersValidator()
        {
            RuleFor(x => x.RentalId).NotNull().WithMessage(configure => $"{nameof(configure.RentalId)} is required.");
            RuleFor(x => x.Start).NotNull().WithMessage(configure => $"{nameof(configure.Start)} is required.");
            RuleFor(x => x.Nights).NotNull().WithMessage(configure => $"{nameof(configure.Nights)} is required.");
            RuleFor(x => x.Nights).NotNull().LessThanOrEqualTo(0).WithMessage(configure => $"{nameof(configure.Nights)} must be positive.");
        }
    }
}
