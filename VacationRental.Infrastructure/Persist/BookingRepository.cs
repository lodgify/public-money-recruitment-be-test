using System.Collections.Generic;
using System.Linq;
using VacationRental.Domain.Entities;
using VacationRental.Domain.Repositories;
using VacationRental.Domain.Values;
using VacationRental.Infrastructure.Persist.Exceptions;
using VacationRental.Infrastructure.Persist.PersistModels;

namespace VacationRental.Infrastructure.Persist
{
    public sealed class BookingRepository : IBookingRepository
    {
        private readonly Dictionary<int, BookingDataModel> _bookings = new Dictionary<int, BookingDataModel>();

        public Booking Get(BookingId id)
        {
            if (_bookings.TryGetValue(id.Id, out var bookingDataModel))
            {
                return MapToDomain(bookingDataModel);
            }

            throw new BookingNotFoundException(id);
        }

        public IReadOnlyCollection<Booking> GetByRentalId(RentalId rentalId) =>
            _bookings.Values.Where(booking => booking.RentalId == rentalId.Id)
                .Select(MapToDomain)
                .ToList();

        public Booking Add(Booking booking)
        {
            var newBookingDataModel = MapToDataModel(booking);
            newBookingDataModel.Id = NextId(); // storage is responsible for generating new IDs. 

            _bookings.Add(newBookingDataModel.Id, newBookingDataModel);

            return MapToDomain(newBookingDataModel); // returns a domain object with the new ID.    
        }

        private int NextId()
        {
            var maxId = _bookings.Keys.Max(id => id);
            return maxId + 1;
        }

        private static BookingDataModel MapToDataModel(Booking booking)
        {
            return new BookingDataModel
            {
                Id = booking.Id.Id,
                RentalId = booking.RentalId.Id,
                Start = booking.Period.Start,
                Nights = booking.Period.Nights
            };
        }

        private static Booking MapToDomain(BookingDataModel dataModel) =>
            new Booking(new BookingId(dataModel.Id), 
                new RentalId(dataModel.RentalId),
                new BookingPeriod(dataModel.Start, dataModel.Nights));
    }
}
