using FluentValidation;

namespace VR.Application.Queries.GetRental
{
    public class GetRentalValidatorCollection : AbstractValidator<GetRentalQuery>
    {
        public GetRentalValidatorCollection()
        {
            RuleFor(r => r.RentalId)
                .GreaterThan(0)
                .WithMessage("Should have valid rentalId");
        }
    }
}
