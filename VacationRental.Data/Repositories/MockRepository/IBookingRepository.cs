using System.Collections.Generic;
using VacationRental.Business;

namespace VacationRental.Data
{
    public interface IBookingRepository
	{
		IList<Booking> GetAll();

		Booking GetById(int Id);
	}
}
