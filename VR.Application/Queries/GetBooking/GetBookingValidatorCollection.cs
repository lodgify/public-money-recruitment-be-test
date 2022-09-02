using FluentValidation;

namespace VR.Application.Queries.GetBooking
{
    public class GetBookingValidatorCollection : AbstractValidator<GetBookingQuery>
    {
        public GetBookingValidatorCollection()
        {
            RuleFor(r => r.BookingId)
                .GreaterThan(0)
                .WithMessage("Should have valid bookingId");
        }
    }
}
