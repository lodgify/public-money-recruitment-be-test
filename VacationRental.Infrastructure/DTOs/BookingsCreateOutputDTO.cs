using Mapster;

namespace VacationRental.Infrastructure.DTOs
{
    public class BookingsCreateOutputDTO
    {
        public int Id { get; set; }
    }

    public class BookingsCreateOutputDTOMapping : IRegister
    {
        public void Register(TypeAdapterConfig config)
        {
            config.ForType<int, BookingsCreateOutputDTO>()
                .Map(dest => dest.Id, src => src);
        }
    }
}
