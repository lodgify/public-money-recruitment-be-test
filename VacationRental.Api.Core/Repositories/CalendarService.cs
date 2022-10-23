using LanguageExt;
using System;
using System.Threading.Tasks;
using VacationRental.Api.Core.Helpers;
using VacationRental.Api.Core.Helpers.Exceptions;
using VacationRental.Api.Core.Interfaces;
using VacationRental.Api.Core.Models;
using VacationRental.Api.Infrastructure.Contracts;

namespace VacationRental.Api.Core.Repositories
{
    public class CalendarService : ICalendarService
    {
        private readonly IBookingRepository _bookingRepository;
        private readonly IRentalRepository _rentalRepository;

        public CalendarService(IBookingRepository bookingRepository, IRentalRepository rentalRepository)
        {
            _bookingRepository = bookingRepository;
            _rentalRepository = rentalRepository;
        }

        public async Task<Result<CalendarViewModel>> GetRentalCalendarAsync(int rentalId, DateTime start, int nights)
        {
            var rentals = await _rentalRepository.GetAllAsync();
            var bookings = await _bookingRepository.GetAllAsync();

            if (nights < 0)
                return await Task.FromResult(new Result<CalendarViewModel>(new NegativeArgumentException("Night argument must be positive")));
            if (!rentals.ContainsKey(rentalId))
                return await Task.FromResult(new Result<CalendarViewModel>(new NotFoundException("Rental not found", rentalId)));

            var calendaryViewModel = CommonHelper.SetCalendarInstanceForRentalId(rentalId);
            var rentalUnit = rentals[rentalId].Units;

            for (var i = 0; i < nights; i++)
            {
                var date = start.SetCalendarDateInstanceFromStartDate(i);

                foreach (var booking in bookings.Values)
                {
                    if (booking.ValidateCalendarBookingsFromDates(date.Date, rentalId))
                    {
                        date.Bookings.Add(new CalendarBookingViewModel { Id = booking.Id, Unit = rentalUnit });
                    }
                }
                calendaryViewModel.Dates.Add(date);
            }
            
            // Set preparation time for rentals
            var preparationTimeInDays = rentals[rentalId].PreparationTimeInDays;
            if(preparationTimeInDays > 0)
            {
                for(var i = 0; i < preparationTimeInDays; i++)
                {
                    var addedDays = i + nights;
                    var date = start.SetCalendarDateInstanceFromStartDate(addedDays);
                    date.PreparationTimes.Add(new CalendarRentalUnitViewModel { Unit = rentalUnit });
                    calendaryViewModel.Dates.Add(date);
                }
            }

            return calendaryViewModel;
        }
    }
}
