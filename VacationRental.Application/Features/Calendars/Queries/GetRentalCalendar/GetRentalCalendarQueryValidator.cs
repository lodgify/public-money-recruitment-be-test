using FluentValidation;

namespace VacationRental.Application.Features.Calendars.Queries.GetRentalCalendar
{
    public class GetRentalCalendarQueryValidator : AbstractValidator<GetRentalCalendarQuery>
    {
        public GetRentalCalendarQueryValidator()
        {
            RuleFor(x => x.Nights)
                .GreaterThan(0).WithMessage("Nights must be positive");

            RuleFor(x => x.RentalId)
                .NotNull().WithMessage("Rental id can not be null")
                .GreaterThan(0).WithMessage("Rental id must be positive"); 
        }
    }
}
