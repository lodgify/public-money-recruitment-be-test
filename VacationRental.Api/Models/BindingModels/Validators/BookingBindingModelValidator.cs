using FluentValidation;

namespace VacationRental.Api.Models
{
    public class BookingBindingModelValidator : AbstractValidator<BookingBindingModel>
    {
        public BookingBindingModelValidator()
        {
            CascadeMode = CascadeMode.Stop;

            RuleFor(x => x.Nights).GreaterThan(0);

            RuleFor(x => x.RentalId).GreaterThan(0);

            RuleFor(x => x.Start).Must(x => x != default);
        }
    }
}
