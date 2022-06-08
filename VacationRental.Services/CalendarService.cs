using AutoMapper;
using Microsoft.Extensions.Logging;
using VacationRental.DataAccess.Interfaces;
using VacationRental.DataAccess.Models.Entities;
using VacationRental.Models.Dtos;
using VacationRental.Models.Exceptions;
using VacationRental.Services.Interfaces;

namespace VacationRental.Services
{
    public class CalendarService : ICalendarService
    {
        private readonly IGenericRepository<Booking> _bookingRepository;
        private readonly IGenericRepository<Rental> _rentalRepository;

        private readonly IMapper _mapper;
        private readonly ILogger<CalendarService> _logger;

        public CalendarService(IGenericRepository<Booking> bookingRepository,
                               IGenericRepository<Rental> rentalRepository,
                               IMapper mapper,
                               ILogger<CalendarService> logger)
        {
            _bookingRepository = bookingRepository;
            _rentalRepository = rentalRepository;

            _mapper = mapper;
            _logger = logger;
        }

        public async Task<CalendarDto> GetCalendarAsync(int rentalId, int nights, DateTime start)
        {
            _logger.LogInformation($"{nameof(GetCalendarAsync)} with params: '{nameof(rentalId)}'={rentalId}, " +
                                                                           $"'{nameof(nights)}'={nights}, " +
                                                                           $"'{nameof(start)}'={start}.");

            var rental = await _rentalRepository.GetByIdAsync(rentalId);
            if (rental == null)
            {
                var message = $"{nameof(Rental)} with Id: {rentalId} not found.";

                _logger.LogError(message);
                throw new RentalNotFoundException(message);
            }

            var result = new CalendarDto
            {
                RentalId = rentalId,
                Dates = Array.Empty<CalendarDateDto>()
            };

            var dates = new List<CalendarDateDto>();

            for (var i = 0; i < nights; i++)
            {
                var date = new CalendarDateDto
                {
                    Date = start.Date.AddDays(i),
                    Bookings = Array.Empty<CalendarBookingDto>()
                };

                var bookings = await _bookingRepository.FindAsync(x => x.RentalId == rentalId && x.Start <= date.Date && x.Start.AddDays(x.Nights) > date.Date);

                date.Bookings = bookings.Select(x => new CalendarBookingDto { Id = x.Id, Unit = rental.Units }).ToArray();
                date.PreparationTimes = bookings.Select(x => new CalendarPreparationTimeDto { Unit = rental.PreparationTimeInDays }).ToArray(); 

                dates.Add(date);
            }

            result.Dates = dates.ToArray();

            _logger.LogInformation($"{nameof(GetCalendarAsync)} with params: '{nameof(rentalId)}'={rentalId}, " +
                                                                           $"'{nameof(nights)}'={nights}, " +
                                                                           $"'{nameof(start)}'={start} was getting successfully.");

            return result;
        }
    }
}
