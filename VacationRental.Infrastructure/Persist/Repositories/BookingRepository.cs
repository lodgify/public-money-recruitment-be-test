using System;
using System.Collections.Generic;
using System.Linq;
using VacationRental.Domain.Entities;
using VacationRental.Domain.Repositories;
using VacationRental.Domain.Values;
using VacationRental.Infrastructure.Persist.Exceptions;
using VacationRental.Infrastructure.Persist.PersistModels;
using VacationRental.Infrastructure.Persist.Storage;

namespace VacationRental.Infrastructure.Persist.Repositories
{
    public sealed class BookingRepository : IBookingRepository
    {
        private readonly IInMemoryDataStorage<BookingDataModel> _storage;

        public BookingRepository(IInMemoryDataStorage<BookingDataModel> storage)
        {
            _storage = storage ?? throw new ArgumentNullException(nameof(storage));
        }

        public Booking Get(BookingId id)
        {
            if (_storage.TryGetValue(id.Id, out var bookingDataModel))
            {
                return MapToDomain(bookingDataModel);
            }

            throw new BookingNotFoundException(id);
        }

        public IReadOnlyCollection<Booking> GetByRentalId(RentalId rentalId) =>
            _storage.Get(booking => booking.RentalId == (int) rentalId)
                .Select(MapToDomain)
                .ToList();

        public Booking Add(Booking booking)
        {
            var newBookingDataModel = MapToDataModel(booking);
            _storage.Add(newBookingDataModel);

            return MapToDomain(newBookingDataModel); // returns a domain object with the new ID.    
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
