using VacationRental.Domain.Models;
using System.Threading.Tasks;

namespace VacationRental.Domain.Interfaces
{
	public interface IRentalBookingService
	{
		public Task<Booking> AddBooking(Booking bookingToAdd);

		public Rental UpdateRental(Rental newRental);
	}
}
