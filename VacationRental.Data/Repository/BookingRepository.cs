using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using VacationRental.Data.IRepository;
using VacationRental.Data.Store;
using VacationRental.Domain.Models;

namespace VacationRental.Data.Repository
{
    public class BookingRepository : BaseRepository, IBookingRepository
    {
        private IDictionary<int, BookingViewModel> _storeContext;

        public BookingRepository(IDataContext context) : base(context)
        {
            _storeContext = context.Bookings;
        }

        public BookingViewModel Add(BookingViewModel entity)
        {
            int id = _storeContext.Count + 1;
            entity.Id = id;

            _storeContext.Add(id, entity);

            return entity;
        }

        public bool Delete(int id)
        {
            _storeContext.Remove(id);
            return true;
        }

        public BookingViewModel GetById(int id)
        {
            if (!_storeContext.ContainsKey(id))
                throw new ApplicationException("Booking not found");

            return _storeContext[id];
        }

        public IEnumerable<BookingViewModel> GetAll()
        {
            return _storeContext.Values.ToList();
        }

        public IReadOnlyCollection<BookingViewModel> GetByRentalId(int rentalId)
        {
            return _storeContext.Values.Where(e => e.RentalId == rentalId).ToList();
        }

        public BookingViewModel Update(BookingViewModel entity)
        {
            if (!_storeContext.ContainsKey(entity.Id))
                throw new ApplicationException("Booking not found");

            BookingViewModel rental = _storeContext[entity.Id];
            rental.Nights = entity.Nights;
            rental.Start = entity.Start;

            return rental;
        }
    }
}
