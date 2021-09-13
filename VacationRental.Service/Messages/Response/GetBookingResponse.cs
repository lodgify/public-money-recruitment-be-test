using VacationRental.Infrastructure;

namespace VacationRental.Application
{
    public class GetBookingResponse : ResponseBase
    {
        public BookingViewModel BookingViewModel { get; set; }
    }
}
