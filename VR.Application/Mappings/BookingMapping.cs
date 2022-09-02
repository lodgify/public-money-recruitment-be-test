using VR.Application.Queries.GetBooking;
using VR.Application.Requests.AddBooking;
using VR.Domain.Models;
using VR.Infrastructure.Mapping.Interfaces;

namespace VR.Application.Mappings
{
    public class BookingMapping : IMapperRegister
    {
        public void Register(IObjectMapper config)
        {
            config.CreateMapConfig<Booking, GetBookingResponse>();
            config.CreateMapConfig<Booking, AddBookingResponse>();
        }
    }
}