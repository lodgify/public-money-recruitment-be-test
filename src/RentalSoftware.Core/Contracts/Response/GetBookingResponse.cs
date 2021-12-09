using RentalSoftware.Core.Entities;

namespace RentalSoftware.Core.Contracts.Response
{
    public class GetBookingResponse : ResponseBase
    {
        public BookingViewModel BookingViewModel { get; set; }
    }
}
