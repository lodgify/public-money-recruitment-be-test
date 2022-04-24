using Mapster;
using VacationRental.Domain.Rentals;

namespace VacationRental.Infrastructure.DTOs
{
    public class RentalCreateInputDTO
    {
        public int Units { get; set; }

        public int PreparationTimeInDays { get; set; }
    }

    public class RentalCreateInputDTOMapping : IRegister
    {
        public void Register(TypeAdapterConfig config)
        {
            config.ForType<RentalCreateInputDTO, Rental>()
                .Map(dest => dest.Units, src => src.Units)
                .Map(dest => dest.PreparationTime, src => src.PreparationTimeInDays);
        }
    }
}
