using FluentValidation;

namespace VacationRental.Application.Features.Bookings.Queries.GetBooking
{
    public class GetBookingQueryValidator : AbstractValidator<GetBookingQuery>
    {
        public GetBookingQueryValidator() 
        {
            RuleFor(c => c.bookingId)
                .NotNull().WithMessage("BookingId can not be null");

        }
    }
}
