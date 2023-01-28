using System.Threading.Tasks;
using VacationRental.Application.Dtos;
using VacationRental.Application.ViewModels;

namespace VacationRental.Application.Midlewares.Rental
{
	public interface IRentalMiddleware
	{
		Task<int> AddRentalWithTimePeriod(RentalDto input);

		Task<RentalViewModel> GetById(int rentalId);
	}
}
