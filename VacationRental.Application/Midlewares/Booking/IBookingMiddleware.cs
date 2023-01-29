using System.Threading.Tasks;
using VacationRental.Application.Dtos;
using VacationRental.Application.ViewModels;

namespace VacationRental.Application.Midlewares.Booking
{
	public interface IBookingMiddleware
	{
		Task<BookingViewModelOutput> GetBookingById(int id);

		Task<int> CreateBooking(BookingInputDto input);
	}
}
