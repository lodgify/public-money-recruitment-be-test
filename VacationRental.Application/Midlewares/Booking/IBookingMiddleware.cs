using System.Threading.Tasks;
using VacationRental.Api.Models;
using VacationRental.Application.Dtos;

namespace VacationRental.Application.Midlewares.Booking
{
	public interface IBookingMiddleware
	{
		Task<BookingViewModelOutput> GetBookingById(int id);

		Task<int> CreateBooking(BookingInputDto input);
	}
}
