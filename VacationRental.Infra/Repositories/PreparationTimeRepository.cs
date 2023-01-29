using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using VacationRental.Domain.PreparationTimes;
using VacationRental.Infra.Repositories.Interfaces;

namespace VacationRental.Infra.Repositories
{
	public class PreparationTimeRepository : IPreparationTimeRepository
	{
		private readonly VacationRentalContext _context;
		public PreparationTimeRepository(VacationRentalContext context)
		{
			this._context = context;
		}

		public async Task<List<PreparationTime>> GetAllPreparationTimeInRental(int rentalId)
		{
			return await this._context.PreparationTimes
				.Include(x => x.Rental)
				.Where(x => x.Rental.Id == rentalId).ToListAsync();
		}
	}
}
