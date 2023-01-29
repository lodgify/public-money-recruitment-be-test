using System.Collections.Generic;
using System.Threading.Tasks;
using VacationRental.Domain.Bookings;

namespace VacationRental.Infra.Repositories.Interfaces
{
	public interface IBookingRepository
	{
		Task<Booking> GetBooking(int id);

		Task<List<Booking>> GetAllBookingsByRentalId(int rentalId);

		Task<Booking> CreateBooking(Booking booking);
	}
}
