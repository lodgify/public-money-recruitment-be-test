﻿using System.Threading.Tasks;
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

		public async Task<int> AddRental(Rental rental)
		{
			try
			{
				await this._context.Rentals.AddAsync(rental);
				var success = await this._context.SaveChangesAsync() > 0;
				if (success)
				{
					return rental.Id;
				}

				return default;
			}
			catch (System.Exception ex)
			{
				throw;
			}
		}
	}
}
