using VR.Application.Queries.GetRental;
using VR.Application.Requests.AddRental;
using VR.Application.Requests.UpdateRental;
using VR.Domain.Models;
using VR.Infrastructure.Mapping.Interfaces;

namespace VR.Application.Mappings
{
    public class RentalMapping : IMapperRegister
    {
        public void Register(IObjectMapper config)
        {
            config.CreateMapConfig<Rental, GetRentalResponse>();
            config.CreateMapConfig<Rental, AddRentalResponse>();
            config.CreateMapConfig<Rental, UpdateRentalResponse>();
        }
    }
}