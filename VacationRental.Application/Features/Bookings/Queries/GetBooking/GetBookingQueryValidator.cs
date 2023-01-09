using FluentValidation;

namespace VacationRental.Application.Features.Bookings.Queries.GetBooking
{
    public class GetBookingQueryValidator : AbstractValidator<GetBookingQuery>
    {
        public GetBookingQueryValidator() 
        {
            RuleFor(c => c.bookingId)
                .NotNull().WithMessage("BookingId can not be null");

            RuleFor(c => c.bookingId)
                .GreaterThan(0).WithMessage("BookingId can not be negative");

        }
    }
}
