using LanguageExt;
using Microsoft.Extensions.Logging;
using System;
using VacationRental.Api.Core.Interfaces;
using VacationRental.Api.Core.Models;
using VacationRental.Api.Interfaces;

namespace VacationRental.Api.Services
{
    public class BookingService : IBookingService
    {
        private readonly IBookingRepository _bookingRepository;
        private readonly ILogger<BookingService> _logger;

        public BookingService(IBookingRepository bookingRepository, ILogger<BookingService> logger)
        {
            _bookingRepository = bookingRepository;
            _logger = logger;
        }

        public Result<BookingViewModel> GetBookingById(int bookingId)
        {
            try
            {
                _logger.LogInformation($"Get data with value: {bookingId}");
                return _bookingRepository.GetBooking(bookingId);
            }
            catch (ApplicationException exception)
            {
                _logger.LogError($"An error occurred for method {nameof(GetBookingById)} with error:\n\n{exception.Message}");
                return new Result<BookingViewModel>(exception);
            }
        }

        public Result<ResourceIdViewModel> AddNewBooking(BookingBindingModel booking)
        {
            try
            {
                _logger.LogInformation($"Insert data with value: {booking}");
                var newBooking = _bookingRepository.InsertNewBooking(booking);
                if (newBooking.Id == 0)
                    return new Result<ResourceIdViewModel>(new ApplicationException("not acceptable"));

                return newBooking;
            }
            catch (ApplicationException exception)
            {
                _logger.LogError($"An error occurred for method {nameof(AddNewBooking)} with error:\n\n{exception.Message}");
                return new Result<ResourceIdViewModel>(exception);
            }
        }
    }
}
