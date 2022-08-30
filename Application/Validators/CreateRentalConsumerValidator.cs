using Application.Models.Rental.Requests;
using FluentValidation;

namespace Application.Validators;

public class CreateRentalConsumerValidator : AbstractValidator<CreateRentalRequest>
{
    public CreateRentalConsumerValidator()
    {
        RuleFor(r => r.Units).GreaterThan(0);
        RuleFor(r => r.Units).GreaterThanOrEqualTo(1);
        RuleFor(r => r.PreparationTimeInDays).GreaterThanOrEqualTo(1);
    }
}