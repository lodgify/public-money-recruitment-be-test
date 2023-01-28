using System.Threading.Tasks;
using VacationRental.Domain.Rentals;

namespace VacationRental.Infra.Repositories.Interfaces
{
	public interface IRentalRepository
	{
		Task<int> AddRental(Rental rental);
	}
}
