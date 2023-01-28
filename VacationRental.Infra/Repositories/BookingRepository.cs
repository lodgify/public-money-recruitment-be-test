using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;
using VacationRental.Domain.Bookings;
using VacationRental.Infra.Repositories.Interfaces;

namespace VacationRental.Infra.Repositories
{
	public class BookingRepository : IBookingRepository
	{
		private readonly VacationRentalContext _context;

		public BookingRepository(VacationRentalContext context)
		{
			this._context = context;
		}

		public async Task<Booking> GetBooking(int id)
		{
			return await this._context.Bookings.FirstOrDefaultAsync(x => x.Id == id);
		}

		public async Task<Booking> CreateBooking(Booking booking)
		{
			try
			{
				await this._context.Bookings.AddAsync(booking);
				var success = await this._context.SaveChangesAsync() > 0;
				if (success) 
				{
					return booking;
				}

				return null;
			}
			catch (System.Exception ex)
			{

				throw;
			}
		}
	}
}
