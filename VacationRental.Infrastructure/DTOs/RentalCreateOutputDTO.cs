using Mapster;

namespace VacationRental.Infrastructure.DTOs
{
    public class RentalCreateOutputDTO
    {
        public int Id { get; set; }
    }

    public class RentalCreateOutputDTOMapping : IRegister
    {
        public void Register(TypeAdapterConfig config)
        {
            config.ForType<int, RentalCreateOutputDTO>()
                .Map(dest => dest.Id, src => src);
        }
    }
}
