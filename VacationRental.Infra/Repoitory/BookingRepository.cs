using VacationRental.Domain.VacationRental.Interfaces.Repositories;
using VacationRental.Domain.VacationRental.Models;

namespace VacationRental.Infra.Repoitory
{
    public class BookingRepository : IBookingRepository
    {
        private readonly DatabaseInMemoryContext _context;
        public BookingRepository(DatabaseInMemoryContext context)
        {
            _context = context;
        }

        public async Task<BookingViewModel> Get(int bookingId)
        {
            return _context.Booking.First(x => x.Id == bookingId);
        }

        public async Task<List<BookingViewModel>> Get()
        {
            return _context.Booking.ToList();
        }

        public async Task<List<BookingViewModel>> GetByRentalId(int rentalId)
        {
            return _context.Booking.Where(x => x.RentalId == rentalId).ToList();
        }

        public async Task<int?> GetLastId()
        {
            var result = _context?.Booking?.Select(x => x.Id).LastOrDefault();
            return result;
        }

        public async Task<ResourceIdViewModel> Post(BookingViewModel bookingModel)
        {
            _context.Booking.Add(bookingModel);
            _context.SaveChanges();

            var key = new ResourceIdViewModel { Id = bookingModel.Id };

            return (key);
        }

        public async Task<ResourceIdViewModel> Put(BookingViewModel bookingModel)
        {
            var bookingTable = _context?.Booking?.First(x => x.Id == bookingModel.Id);
            bookingTable.End = bookingModel.End;

            _context.Entry(bookingTable).State = Microsoft.EntityFrameworkCore.EntityState.Modified;
            _context.SaveChanges();

            var key = new ResourceIdViewModel { Id = bookingTable.Id };

            return (key);
        }
    }
}
