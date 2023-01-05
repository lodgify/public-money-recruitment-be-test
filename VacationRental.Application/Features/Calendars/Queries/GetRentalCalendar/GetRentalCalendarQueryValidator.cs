using FluentValidation;

namespace VacationRental.Application.Features.Calendars.Queries.GetRentalCalendar
{
    public class GetRentalCalendarQueryValidator : AbstractValidator<GetRentalCalendarQuery>
    {
        public GetRentalCalendarQueryValidator()
        {
            RuleFor(x => x.Nights)
                .GreaterThanOrEqualTo(0).WithMessage("Nights must be positive");

            RuleFor(x => x.RentalId)
                .NotNull().WithMessage("Rental id can not be null");
        }
    }
}
