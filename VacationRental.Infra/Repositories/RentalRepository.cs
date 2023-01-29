using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;
using VacationRental.Domain.Rentals;
using VacationRental.Infra.Repositories.Interfaces;

namespace VacationRental.Infra.Repositories
{
	public class RentalRepository : IRentalRepository
	{
		private readonly VacationRentalContext _context;

		public RentalRepository(VacationRentalContext context)
		{
			this._context = context;
		}

		public async Task<Rental> GetById(int rentalId)
		{
			return await this._context.Rentals
				.Include(x => x.Bookings)
				.Include(x => x.PreparationTimes)
				.FirstOrDefaultAsync(x => x.Id == rentalId);
		}

		public async Task<int> AddRental(Rental rental)
		{
			await this._context.Rentals.AddAsync(rental);
			var success = await this._context.SaveChangesAsync() > 0;
			if (success)
			{
				return rental.Id;
			}

			return default;
		}
	}
}
