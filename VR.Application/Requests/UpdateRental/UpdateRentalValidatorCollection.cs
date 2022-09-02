using FluentValidation;

namespace VR.Application.Requests.UpdateRental
{
    public class UpdateRentalValidatorCollection : AbstractValidator<UpdateRentalRequest>
    {
        public UpdateRentalValidatorCollection()
        {
            RuleFor(r => r.Id)
                .NotEmpty()
                .WithMessage("Provide rental id");
            RuleFor(r => r.Units)
                .NotEmpty()
                .GreaterThan(0)
                .WithMessage("Units must be greater then 0");
            RuleFor(r => r.PreparationTimeInDays)
                .GreaterThan(-1)
                .WithMessage("PreparationTimeInDays must be positive");
        }
    }
}
