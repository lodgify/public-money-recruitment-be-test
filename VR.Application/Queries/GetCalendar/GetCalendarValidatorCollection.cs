using FluentValidation;

namespace VR.Application.Queries.GetCalendar
{
    public class GetCalendarValidatorCollection : AbstractValidator<GetCalendarQuery>
    {
        public GetCalendarValidatorCollection()
        {
            RuleFor(r => r.Nights)
                .GreaterThan(0)
                .WithMessage("Nights must be greater then 0");
            RuleFor(r => r.RentalId)
                .NotEmpty()
                .WithMessage("Should have valid rentalId");
        }
    }
}
