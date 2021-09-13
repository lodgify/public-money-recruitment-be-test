using System.Collections.Generic;
using VacationRental.Business;

namespace VacationRental.Data
{
	public interface IRentalRepository
	{
		void Add(Rental rental);

		void Delete(long id);

		void Update(Rental rental);

		IList<Rental> GetAll();

		Rental GetById(int Id);

		Booking AddBooking(Booking booking);
	}
}
