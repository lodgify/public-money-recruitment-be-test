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
}
