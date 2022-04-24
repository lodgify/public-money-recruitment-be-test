using FluentValidation;
using Mapster;
using VacationRental.Domain.Rentals;

namespace VacationRental.Infrastructure.DTOs
{
    public class RentalUpdateInputDTO
    {
        public RentalUpdateInputDTO()
        {
        }

        public int Units { get; set; }

        public int PreparationTimeInDays { get; set; }
    }

    public class RentalUpdateInputDTOMapping : IRegister
    {
        public void Register(TypeAdapterConfig config)
        {
            config.ForType<RentalUpdateInputDTO, Rental>()
                .Map(dest => dest.Units, src => src.Units)
                .Map(dest => dest.PreparationTime, src => src.PreparationTimeInDays);
        }
    }

    public class RentalUpdateInputDTOValidator : AbstractValidator<RentalUpdateInputDTO>
    {
        public RentalUpdateInputDTOValidator()
        {
            RuleFor(x => x.Units).GreaterThan(0).WithMessage("Rental should at least have 1 Unit");
            RuleFor(x => x.PreparationTimeInDays).GreaterThan(0).WithMessage("Preparation Time cannot be less than 1 days");
        }
    }
}
