using System.Collections.Generic;
using System.Linq;
using VacationRental.Business;
namespace VacationRental.Data
{
    public class RentalRepository :IRentalRepository
	{
		private IList<Rental> _rentalList;
		private IList<Booking> _bookingList;

		public RentalRepository(IList<Rental> rentalList, IList<Booking> bookingList)
		{
			_rentalList = rentalList;
			_bookingList = bookingList;
		}

		public void Add(Rental rental)
		{
			int nextId = _rentalList.Count();

			rental.Id = nextId + 1;

			rental.BookingCollection = new List<Booking>();

			_rentalList.Add(rental);
		}

		public Booking AddBooking(Booking booking)		
		{
			int nextId = _bookingList.Count();

			booking.Id = nextId + 1;

			_bookingList.Add(booking);

			_rentalList.Where(w => w.Id == booking.Rental.Id).FirstOrDefault().BookingCollection.Add(booking);

			return booking;
		}


		public IList<Rental> GetAll()
		{
			return _rentalList;
		}

		public Rental GetById(int Id)
		{			
			var tmpRentalList = _rentalList.Where(w => w.Id == Id);
			
			if (tmpRentalList.Count() == 0)
				return null;

			return tmpRentalList.FirstOrDefault();
		}

		public void Delete(long id)
		{
			_rentalList.Remove(_rentalList.Where(w => w.Id == id).FirstOrDefault());
		}

		public void Update(Rental rental)
		{
			Rental rentalForUpdate = _rentalList.Where(w => w.Id == rental.Id).FirstOrDefault();

			rentalForUpdate.Units = rental.Units;
			rentalForUpdate.PreparationTimeInDays = rental.PreparationTimeInDays;
			rentalForUpdate.RentalType = rental.RentalType;
		}
	}
}