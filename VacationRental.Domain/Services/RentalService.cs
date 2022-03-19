using VacationRental.Domain.Models;
using VacationRental.Domain.Interfaces;
using System.Linq;
using System.Threading.Tasks;

namespace VacationRental.Domain.Services
{
	/// <summary>
	/// This class will be registered as a service and will handle the domain's Rental logic
	/// </summary>
	public class RentalService : IRentalService
	{
		private readonly GIRepository<Rental> repository;

		/// Rental Service constructor.
		public RentalService(GIRepository<Rental> repository)
		{
			this.repository = repository;
		}

		/// Task that stores a booking in DB.
		public async Task<Rental> AddAsync(Rental rentalToAdd)
		{
			await repository.Add(rentalToAdd);
			await repository.Save();
			return rentalToAdd;
		}

		public Rental GetById(int rentalId)
		{
			var rental = repository.Query.Where(x=>x.Id==rentalId).FirstOrDefault();
			return rental;
		}

		public Rental Update(Rental rental)
		{
			repository.Update(rental);
			repository.Save();
			return rental;
		}
	}
}
