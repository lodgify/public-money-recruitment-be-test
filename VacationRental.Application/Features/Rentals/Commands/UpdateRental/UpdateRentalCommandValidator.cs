using FluentValidation;

namespace VacationRental.Application.Features.Rentals.Commands.UpdateRental
{
    public class UpdateRentalCommandValidator : AbstractValidator<UpdateRentalCommand>
    {
        public UpdateRentalCommandValidator()
        {
            RuleFor(x => x.RentalId)
                .NotNull().WithMessage("Id can not be null")
                .GreaterThan(0).WithMessage("Units can not be negative");

            RuleFor(x => x.Units)
               .GreaterThan(0).WithMessage("Units can not be negative");

            RuleFor(x => x.PreparationTimeInDays)
                .GreaterThanOrEqualTo(0).WithMessage("Preparation days can not be negative");
        }
        
    }
}
