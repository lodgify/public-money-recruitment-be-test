using Application.Business.Commands.Rental;
using FluentValidation;

namespace Application.Validators.Rental
{
    public class CreateRentalCommandValidator : AbstractValidator<CreateRentalCommand>
    {
        public CreateRentalCommandValidator()
        {
            RuleFor(r => r.Request.Units).GreaterThan(0);
            RuleFor(r => r.Request.PreparationTimeInDays).GreaterThan(0);
        }
    }
}