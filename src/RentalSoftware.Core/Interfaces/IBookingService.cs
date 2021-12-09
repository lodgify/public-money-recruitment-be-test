using RentalSoftware.Core.Contracts.Request;
using RentalSoftware.Core.Contracts.Response;
using RentalSoftware.Core.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace RentalSoftware.Core.Interfaces
{
    public interface IBookingService
    {
        Task<IEnumerable<Booking>> GetAll();
        Task<AddBookingResponse> AddBooking(AddBookingRequest request);
        Task<GetBookingResponse> GetBooking(GetBookingRequest request);
    }
}
