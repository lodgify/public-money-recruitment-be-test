using FluentValidation;
using VacationRental.Data;

namespace VacationRental.Domain.Rentals
{
    public class Rental : BaseEntity
    {
        private readonly RentalValidator _rentalValidator = new RentalValidator();

        public Rental()
        {
            _rentalValidator.ValidateAndThrow(this);
        }

        public Rental(int units, int preparationTime)
        {
            Units = units;
            PreparationTime = preparationTime;

            _rentalValidator.ValidateAndThrow(this);
        }

        public int Units { get; set; }

        public int PreparationTime { get; set; }
    }

    public class RentalValidator : AbstractValidator<Rental>
    {
        public RentalValidator()
        {
            RuleFor(x => x.Id).NotNull();
            RuleFor(x => x.Units).GreaterThan(0).WithMessage("Rental should at least have 1 Unit");
            RuleFor(x => x.PreparationTime).GreaterThan(0).WithMessage("Preparation Time cannot be less than 1 days");
        }
    }
}
