using Mapster;
using VacationRental.Data;
using VacationRental.Domain.Bookings;
using VacationRental.Domain.Rentals;
using VacationRental.Infrastructure.DTOs;
using VacationRental.Infrastructure.Services.Interfaces;

namespace VacationRental.Infrastructure.Services
{
    public class CalendarService : ICalendarService
    {
        private readonly IEntityRepository<Booking> _bookingRepository;
        private readonly IEntityRepository<Rental> _rentalRepository;

        public CalendarService(IEntityRepository<Rental> rentalRepository, IEntityRepository<Booking> bookingRepository)
        {
            _rentalRepository = rentalRepository;
            _bookingRepository = bookingRepository;
        }

        public CalendarDTO GetCalendar(int rentalId, DateTime start, int nights)
        {
            try
            {
                if (nights < 1)
                {
                    throw new ApplicationException("Nights must be positive");
                }

                var rental = _rentalRepository.GetEntityById(rentalId);

                var result = new CalendarDTO
                {
                    RentalId = rental.Id,
                    Dates = new(),
                };

                for(var i = 0; i < nights; i++)
                {
                    var date = start.Date.AddDays(i);
                    CalendarDateDTO calendarDateDTO = new()
                    {
                        Date = date,
                        Bookings = GetBookingsByDate(rental.Id, date),
                        PreparationTimes = GetPreparationDatesByDate(rental.Id, date, rental.PreparationTime)
                    };

                    result.Dates.Add(calendarDateDTO);
                }

                return result;
            }
            catch (Exception)
            {

                throw;
            }
        }

        private List<CalendarBookingDTO> GetBookingsByDate(int rentalId, DateTime date)
        {
            var overlapsingBookings = _bookingRepository
                    .FindEntities(x => x.RentalId == rentalId && DoesBookingDateOverlapse(x, date))
                    .Select(x => x.Adapt<CalendarBookingDTO>())
                    .ToList();

            return overlapsingBookings;
        }

        private bool DoesBookingDateOverlapse(Booking booking, DateTime date)
        {
            return booking.Start <= date &&
                    booking.Start.AddDays(booking.Nights) > date;  
        }

        private List<CalendarPreparationTimeDTO> GetPreparationDatesByDate(int rentalId, DateTime date, int preparationTime)
        {
            var overlapsingPreparationDates = _bookingRepository
                    .FindEntities(x => x.RentalId == rentalId && DoesPreparationTimeOverlapse(x, date, preparationTime))
                    .Select(x => x.Adapt<CalendarPreparationTimeDTO>())
                    .ToList();

            return overlapsingPreparationDates;
        }

        private bool DoesPreparationTimeOverlapse(Booking booking, DateTime date, int preparationTime)
        {
            return booking.Start.AddDays(booking.Nights) <= date &&
                    booking.Start.AddDays(booking.Nights + preparationTime) > date;
        }
    }
}
