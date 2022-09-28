using FluentValidation;
using VacationRental.BusinessObjects;

namespace VacationRental.BusinessLogic.Services.Validators
{
    public class CreateBookingValidator : AbstractValidator<CreateBooking>
    {
        public CreateBookingValidator()
        {
            RuleFor(x => x.Nights).GreaterThan(0);
        }
    }
}
