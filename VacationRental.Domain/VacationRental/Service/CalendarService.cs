using System.Runtime.Serialization;
using VacationRental.Domain.Extensions.Common;
using VacationRental.Domain.VacationRental.Extensions.Enum;
using VacationRental.Domain.VacationRental.Interfaces;
using VacationRental.Domain.VacationRental.Models;
using VacationRental.Domain.VacationRental.Utils;
using static VacationRental.Domain.VacationRental.Models.CalendarDateViewModel;

namespace VacationRental.Domain.VacationRental.Service
{
    public class CalendarService : ICalendarService
    {
        private readonly IRentalsService _rentalsService;
        private readonly IBookingService _bookingService;
        public CalendarService(IRentalsService paramRentalsService, IBookingService paramBookingService)
        {
            _rentalsService = paramRentalsService;
            _bookingService = paramBookingService;
        }

        public async Task<CalendarViewModel> Get(int rentalId, DateTime start, int nights)
        {
            if (nights <= 0)
                throw new ConflictException(EnumExceptions.NightsConflict.GetAttributeOfType<EnumMemberAttribute>().Value);

            var rentals = await _rentalsService.Get(rentalId);

            var bookings = await _bookingService.GetByRentalId(rentalId);

            var result = new CalendarViewModel
            {
                RentalId = rentalId,
                Dates = new List<CalendarDateViewModel>()
            };

            for (var i = 0; i < nights; i++)
            {
                var date = new CalendarDateViewModel
                {
                    Date = start.Date.AddDays(i + rentals.PreparationTimeInDays),
                    Bookings = new List<CalendarBookingViewModel>(),
                    PreparationTimes = new List<Units>()
                };                

                foreach (var booking in bookings)
                {
                    if (booking.Start <= date.Date && booking.End >= date.Date)
                    {
                        date.Bookings.Add(new CalendarBookingViewModel { Id = booking.Id, Unit = booking.Unit });
                        date.PreparationTimes.Add(new Units { Unit = booking.Unit });
                    }
                }

                result.Dates.Add(date);
            }

            return result;
        }
    }
}
