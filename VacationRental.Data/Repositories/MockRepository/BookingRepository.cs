using System.Collections.Generic;
using System.Linq;
using VacationRental.Business;

namespace VacationRental.Data
{
    public class BookingRepository : IBookingRepository
	{		
		private IList<Booking> _bookingList;

		public BookingRepository(IList<Booking> bookingList)
		{
			_bookingList = bookingList;
		}

		public IList<Booking> GetAll()
		{
			return _bookingList;
		}

		public Booking GetById(int Id)
		{
			var tmpBookingList = _bookingList.Where(w => w.Id == Id);

			if (tmpBookingList.Count() == 0)
				return null;

			return tmpBookingList.FirstOrDefault();
		}
	
	}
}
