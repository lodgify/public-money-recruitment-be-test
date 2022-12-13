using System.Collections.Generic;
using System.Linq;
using VacationRental.Api.Data.Interfaces;
using VacationRental.Api.Models;

namespace VacationRental.Api.Data.Repositories
{
    public class BookingRepository : IBookingRepository
    {
        private readonly IDataContext _dataContext;

        public BookingRepository(IDataContext dataContext)
        {
            _dataContext = dataContext;
        }

        public BookingViewModel Get(int id) => _dataContext.Bookings[id];

        public IEnumerable<BookingViewModel> GetBookingsByRentalId(int id)
        {
            return _dataContext.Bookings.Values.Where(p => p.RentalId == id).ToList();
        }

        public int Insert(BookingViewModel model)
        {
            model.Id = _dataContext.BookingId;
            _dataContext.Bookings.TryAdd(model.Id, model);

            return model.Id;
        }

        public bool HasValue(int id)
        {
            return _dataContext.Bookings.ContainsKey(id);
        }
    }
}
