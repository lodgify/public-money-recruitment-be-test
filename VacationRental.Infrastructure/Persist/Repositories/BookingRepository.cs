using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using VacationRental.Domain.Entities;
using VacationRental.Domain.Exceptions;
using VacationRental.Domain.Repositories;
using VacationRental.Domain.Values;
using VacationRental.Infrastructure.Persist.PersistModels;
using VacationRental.Infrastructure.Persist.Storage;

namespace VacationRental.Infrastructure.Persist.Repositories
{
    public sealed class BookingRepository : InMemoryRepository<Booking, BookingId, BookingDataModel>, IBookingRepository
    {

        public BookingRepository(IInMemoryDataStorage<BookingDataModel> storage) : base(storage)
        {

        }

        public ValueTask<IReadOnlyCollection<Booking>> GetByRentalId(RentalId rentalId) =>
            Get(booking => booking.RentalId == (int) rentalId);

        protected override BookingDataModel MapToDataModel(Booking booking)
        {
            return new BookingDataModel
            {
                Id = booking.Id.Id,
                RentalId = booking.RentalId.Id,
                Period = new TimePeriodDataModel { Start =  booking.Period.Start, Days = booking.Period.Nights},
                PreparationInDays = booking.Preparation.Days,
                Unit = booking.Unit
            };
        }

        protected override int RetrieveId(BookingId id) => id.Id;

        protected override Exception GetNotFoundException(BookingId id) =>
            new BookingNotFoundException(id);

        protected override Booking MapToDomain(BookingDataModel dataModel)
        {
            return new Booking(new BookingId(dataModel.Id),
                new RentalId(dataModel.RentalId),
                new BookingPeriod(dataModel.Period.Start, dataModel.Period.Days),
                dataModel.PreparationInDays,
                dataModel.Unit);
        }
    }
}
