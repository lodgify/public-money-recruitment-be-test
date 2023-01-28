using System.Threading.Tasks;
using VacationRental.Application.Dtos;

namespace VacationRental.Application.Midlewares.Rental
{
	public interface IRentalMiddleware
	{
		Task<int> AddRentalWithTimePeriod(RentalDto input);
	}
}
