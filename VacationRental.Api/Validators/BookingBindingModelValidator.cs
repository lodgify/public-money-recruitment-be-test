using FluentValidation;
using VacationRental.Api.Models;

namespace VacationRental.Api.Validators;

public sealed class BookingBindingModelValidator : AbstractValidator<BookingBindingModel>
{
    public BookingBindingModelValidator()
    {
        RuleFor(x => x.Nights).GreaterThan(0).WithMessage("Nights must be positive");
    }
}