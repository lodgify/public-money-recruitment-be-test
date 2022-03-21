using VacationRental.Domain.Models;
using System.Threading.Tasks;

namespace VacationRental.Domain.Interfaces
{
	public interface IRentalService
	{
		public Task<Rental> AddAsync(Rental rentalToAdd);

		public Rental GetById(int rentalId);

		public Rental Update(Rental rental);
	}
}