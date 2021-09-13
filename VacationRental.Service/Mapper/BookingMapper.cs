using VacationRental.Business;

namespace VacationRental.Application
{
    public static class BookingMapper
    {
        public static BookingViewModel ConvertToBookingViewModel(this Booking booking)
        {
            BookingViewModel bookingViewModel = new BookingViewModel();

            bookingViewModel.Id = booking.Id;
            bookingViewModel.Nights = booking.NumberOfNights;
            bookingViewModel.RentalId = booking.Rental.Id;
            bookingViewModel.Start = booking.StartDate;

            return bookingViewModel;
        }     
    }
}