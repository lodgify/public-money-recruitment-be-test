using FluentValidation;

namespace VacationRental.Application.Queries.Calendar
{
    public class BookingCalendarQueryValidator : AbstractValidator<BookingCalendarForRentalQuery>
    {
        public BookingCalendarQueryValidator()
        {
            RuleFor(query => query.Nights)
                .GreaterThan(0)
                .WithMessage($"'{nameof(BookingCalendarForRentalQuery.Nights)}' can't be less than 1");
        }
    }
}
