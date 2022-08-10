using System.Collections.Generic;
using System.Linq;
using VacationRental.Api.DAL.Interfaces;
using VacationRental.Api.Models;

namespace VacationRental.Api.DAL.Repositories
{
    public class BookingRepository : IBookingRepository
    {
        private readonly IDataContext _dataContext;

        public BookingRepository(IDataContext dataContext)
        {
            _dataContext = dataContext;
        }

        public int Add(BookingViewModel model)
        {
            model.Id = _dataContext.BookingId;
            _dataContext.Bookings.Add(model.Id, model);

            return model.Id;
        }

        public BookingViewModel Get(int id) => _dataContext.Bookings[id];

        public IEnumerable<BookingViewModel> GetBookingsByRentalId(int id)
        {
            return _dataContext.Bookings.Values.Where(p => p.RentalId == id).ToList();
        }

        public bool HasValue(int id)
        {
            return _dataContext.Bookings.ContainsKey(id);
        }
    }
}
