using Models.ViewModels;

namespace VacationRental.Api.Operations.BookingOperations
{
    public sealed class BookingGetOperation : IBookingGetOperation
    {
        public BookingGetOperation()
        {
        }

        public BookingViewModel ExecuteAsync(int bookingId)
        {
            if (bookingId <= 0)
                throw new ApplicationException("Wrong Id");

            return DoExecute(bookingId);
        }

        private BookingViewModel DoExecute(int bookingId)
        {
            if (!_bookings.ContainsKey(bookingId))
                throw new ApplicationException("Booking not found");

            return _bookings[bookingId];
        }
    }
}
