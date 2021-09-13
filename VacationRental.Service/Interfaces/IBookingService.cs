namespace VacationRental.Application
{
    public interface IBookingService
    {
        AddBookingResponse AddBooking(AddBookingRequest request);

        GetBookingResponse GetBooking(GetBookingRequest request);
    }
}
