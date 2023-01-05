using FluentValidation;

namespace VacationRental.Application.Features.Rentals.Commands.CreateRental
{
    public class CreateRentalCommandValidator : AbstractValidator<CreateRentalCommand>
    {
        public CreateRentalCommandValidator()
        {
            RuleFor(x => x.Units)
                .GreaterThanOrEqualTo(0).WithMessage("Units can not be negative");

            RuleFor(x => x.PreparationTimeInDays)
                .GreaterThanOrEqualTo(0).WithMessage("Preparation days can not be negative");
        }
    }
}
