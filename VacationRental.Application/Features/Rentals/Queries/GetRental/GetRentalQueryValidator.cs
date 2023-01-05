using FluentValidation;

namespace VacationRental.Application.Features.Rentals.Queries.GetRental
{
    public class GetRentalQueryValidator : AbstractValidator<GetRentalQuery>
    {
        public GetRentalQueryValidator()
        {
            RuleFor(x => x.RentalId)
                .NotNull().WithMessage("Rental id can not be null");
        }
    }
}
