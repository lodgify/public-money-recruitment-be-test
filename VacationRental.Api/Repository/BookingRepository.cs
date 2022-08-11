using System;
using System.Collections.Generic;
using System.Linq;
using VacationRental.Api.Models;

namespace VacationRental.Api.Repository
{
    public class BookingRepository : IBookingRepository
    {
        private readonly IDictionary<int, BookingViewModel> _bookings;

        public BookingRepository(IDictionary<int, BookingViewModel> bookings)
        {
            _bookings = bookings;
        }

        public int BookingCount() => _bookings.Keys.Count;

        public BookingViewModel GetBooking(int id) =>
            _bookings.FirstOrDefault(x => x.Key == id).Value;

        public int CreateBooking(BookingViewModel model)
        {
            _bookings.Add(model.Id, model);

            return model.Id;
        }

        public bool HasRentalBooking(
            int rentalId,
            DateTime startDate,
            DateTime endDate,
            int PreparationTimeInDays
        )
        {
            return _bookings.Values.Any(
                x =>
                    x.RentalId == rentalId
                    && (
                        (
                            x.Start >= startDate
                            || x.Start.AddDays(x.Nights + (PreparationTimeInDays - 1)) >= startDate
                        )
                        && x.Start.AddDays(x.Nights) <= endDate
                    )
            );
        }

        public BookingViewModel GetBooking(int rentalId, DateTime date)
            => _bookings.Values.FirstOrDefault(
                x => x.RentalId == rentalId && x.Start <= date && x.Start.AddDays(x.Nights) > date
            );
        

        public List<DateTime> GetPreparationTimes(
            int rentalId,
            DateTime startDate,
            DateTime endDate,
            int preparationTimesDays
        )
        {
            List<DateTime> preparationTimes = new List<DateTime>();
            var bookingsEndDate = _bookings.Values
                .Where(
                    x =>
                        x.RentalId == rentalId
                        && x.Start >= startDate
                        && x.Start.AddDays(x.Nights) > startDate
                        && x.Start <= endDate
                )
                .Select(x => x.Start.Date.AddDays(x.Nights))
                .ToList();

            if (bookingsEndDate.Count == 0)
                return preparationTimes;

            bookingsEndDate.ForEach(date =>
            {
                for (int i = 0; i < preparationTimesDays; i++)
                    preparationTimes.Add(date.AddDays(i));
            });

            return preparationTimes;
        }
    }
}
