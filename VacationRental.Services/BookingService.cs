using AutoMapper;
using Microsoft.Extensions.Logging;
using VacationRental.DataAccess.Interfaces;
using VacationRental.DataAccess.Models.Entities;
using VacationRental.Models.Dtos;
using VacationRental.Models.Exceptions;
using VacationRental.Models.Paramaters;
using VacationRental.Services.Interfaces;

namespace VacationRental.Services
{
    public class BookingService : IBookingService
    {
        private readonly IGenericRepository<Booking> _bookingRepository;
        private readonly IGenericRepository<Rental> _rentalRepository;
        
        private readonly IMapper _mapper;
        private readonly ILogger<BookingService> _logger;

        public BookingService(IGenericRepository<Booking> bookingRepository, 
                              IGenericRepository<Rental> rentalsRepository, 
                              IMapper mapper, 
                              ILogger<BookingService> logger)
        {
            _bookingRepository = bookingRepository;
            _rentalRepository = rentalsRepository;

            _mapper = mapper;
            _logger = logger;
        }

        public async Task<BookingDto> GetBookingByIdAsync(int bookingId)
        {
            _logger.LogInformation($"{nameof(GetBookingByIdAsync)} with params: '{nameof(bookingId)}'={bookingId}.", bookingId);

            var booking = await _bookingRepository.GetByIdAsync(bookingId);
            if (booking == null)
            {
                var message = $"{nameof(Booking)} with Id: {bookingId} not found.";
                
                _logger.LogError(message);
                throw new BookingNotFoundException(message);
            }

            var result = _mapper.Map<BookingDto>(booking);

            _logger.LogInformation($"{nameof(Booking)} with Id: {bookingId} was getting successfully.", bookingId);

            return result;
        }

        public async Task<BaseEntityDto> AddBookingAsync(BookingParameters parameters)
        {
            _logger.LogInformation($"{nameof(AddBookingAsync)} with params: '{nameof(parameters.RentalId)}'={parameters.RentalId}, " +
                                                                          $"'{nameof(parameters.Start)}'={parameters.Start}, " +
                                                                          $"'{nameof(parameters.Nights)}'={parameters.Nights}.");

            var booking = _mapper.Map<Booking>(parameters);

            var rental = await _rentalRepository.GetByIdAsync(booking.RentalId);
            if (rental == null)
            {
                var message = $"{nameof(Rental)} with Id: {booking.RentalId} not found.";

                _logger.LogError(message);
                throw new RentalNotFoundException(message);
            }

            var count = (await _bookingRepository.FindAsync(x => x.RentalId == booking.RentalId &&
                                                                ((booking.Start <= booking.Start.Date && booking.Start.AddDays(booking.Nights) > booking.Start.Date) ||
                                                                (booking.Start < booking.Start.AddDays(booking.Nights) && booking.Start.AddDays(booking.Nights) >= booking.Start.AddDays(booking.Nights)) ||
                                                                (booking.Start > booking.Start && booking.Start.AddDays(booking.Nights) < booking.Start.AddDays(booking.Nights))))).Count();

            if (count >= rental.Units)
            {
                var message = $"{nameof(Booking)} not available.";

                _logger.LogError(message);
                throw new BookingInvalidException(message);
            }

            await _bookingRepository.AddAsync(booking);

            _logger.LogInformation($"{nameof(Booking)} with Id: {booking.Id} was created successfully.");

            var result = _mapper.Map<BookingDto>(booking);

            return result;
        }
    }
}
