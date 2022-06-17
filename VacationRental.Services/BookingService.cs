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

        public async Task<IEnumerable<BookingDto>> GetBookingsAsync()
        {
            _logger.LogInformation($"{nameof(GetBookingByIdAsync)}");

            var bookings = await _bookingRepository.FindAsync(x => x.IsActive);

            var result = _mapper.Map<IEnumerable<BookingDto>>(bookings);

            return result;
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

            await ValidateBookingAsync(rental, booking);

            await _bookingRepository.AddAsync(booking);

            _logger.LogInformation($"{nameof(Booking)} with Id: {booking.Id} was created successfully.");

            var result = _mapper.Map<BookingDto>(booking);

            return result;
        }

        public async Task UpdateBookingAsync(int bookingId, BookingParameters parameters) 
        {
            _logger.LogInformation($"{nameof(UpdateBookingAsync)} with Id: {bookingId} and with params: '{nameof(parameters.RentalId)}'={parameters.RentalId}, " +
                                                                          $"'{nameof(parameters.Start)}'={parameters.Start}, " +
                                                                          $"'{nameof(parameters.Nights)}'={parameters.Nights}.");

            var entity = await _bookingRepository.GetByIdAsync(bookingId);
            if (entity == null)
            {
                var message = $"{nameof(Booking)} with Id: {bookingId} not found.";

                _logger.LogError(message);
                throw new BookingNotFoundException(message);
            }

            var booking = _mapper.Map<Booking>(parameters);

            entity.Start = booking.Start;
            entity.RentalId = booking.RentalId;
            entity.Nights = booking.Nights;
            entity.IsActive = true;
            entity.Modified = DateTime.UtcNow;

            var rental = await _rentalRepository.GetByIdAsync(entity.RentalId);
            if (rental == null)
            {
                var message = $"{nameof(Rental)} with Id: {entity.RentalId} not found.";

                _logger.LogError(message);
                throw new RentalNotFoundException(message);
            }

            await ValidateBookingAsync(rental, entity);

            await _bookingRepository.UpdateAsync(entity);

            _logger.LogInformation($"{nameof(Booking)} with Id: {bookingId} was created successfully.");
        }

        public async Task DeleteBookingAsync(int bookingId)
        {
            _logger.LogInformation($"{nameof(DeleteBookingAsync)} with Id: {bookingId}.");

            var booking = await _bookingRepository.GetByIdAsync(bookingId);
            if (booking == null)
            {
                var message = $"{nameof(Booking)} with Id: {bookingId} not found.";

                _logger.LogError(message);
                throw new RentalNotFoundException(message);
            }

            booking.IsActive = false;

            await _bookingRepository.UpdateAsync(booking);

            _logger.LogInformation($"{nameof(Booking)} with Id: {booking.Id} was deleted successfully.");
        }

        private async Task ValidateBookingAsync(Rental rental, Booking booking)
        {
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
        }
    }
}
