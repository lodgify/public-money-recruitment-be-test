using System.Collections.Generic;
using System.Threading.Tasks;
using VacationRental.Domain.PreparationTimes;

namespace VacationRental.Infra.Repositories.Interfaces
{
	public interface IPreparationTimeRepository
	{
		Task<List<PreparationTime>> GetAllPreparationTimeInRental(int rentalId);
	}
}
