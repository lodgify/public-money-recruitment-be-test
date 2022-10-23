using LanguageExt;
using System;
using System.Threading.Tasks;
using VacationRental.Api.Core.Helpers;
using VacationRental.Api.Core.Helpers.Exceptions;
using VacationRental.Api.Core.Interfaces;
using VacationRental.Api.Core.Models;
using VacationRental.Api.Infrastructure.Contracts;
using VacationRental.Api.Infrastructure.Models;

namespace VacationRental.Api.Core.Repositories
{
    public class BookingService : IBookingService
    {
        private readonly IBookingRepository _bookingRepository;
        private readonly IRentalRepository _rentalRepository;
        public BookingService(IBookingRepository bookingRepository, IRentalRepository rentalRepository)
        {
            _bookingRepository = bookingRepository;
            _rentalRepository = rentalRepository;
        }

        public async Task<Result<BookingViewModel>> GetBookingByIdAsync(int bookingId)
        {
            var booking = await _bookingRepository.GetByIdAsync(bookingId);
            if(booking == null)
                return await Task.FromResult(new Result<BookingViewModel>(new NotFoundException("Rental not found", bookingId)));
            
            return booking;
        }

        public async Task<Result<ResourceIdViewModel>> InsertNewBookingAsync(BookingBindingModel newBooking)
        {
            var currentRentals = await _rentalRepository.GetAllAsync();
            var currentBookings = await _bookingRepository.GetAllAsync();

            if (newBooking.Nights <= 0)
                return await Task.FromResult(new Result<ResourceIdViewModel>(new NegativeArgumentException("Booking nights must be positive")));
            if(currentRentals[newBooking.RentalId] == null)
                return await Task.FromResult(new Result<ResourceIdViewModel>(new NotFoundException("Rental not found", newBooking.RentalId)));

            // checks every rental available for the night
            for (var i = 0; i < newBooking.Nights; i++)
            {
                var occupiedUnits = 0;
                foreach (var booking in currentBookings.Values)
                {
                    if (CommonHelper.CheckOccupancyAvailability(booking, newBooking, currentRentals[newBooking.RentalId].PreparationTimeInDays))
                        occupiedUnits++;
                }

                if (occupiedUnits >= currentRentals[newBooking.RentalId].Units)
                    return await Task.FromResult(new Result<ResourceIdViewModel>(new NotAvailableException("Current unit is not available", newBooking.RentalId)));
            }

            return await _bookingRepository.AddAsync(newBooking.ToBookingDto());
        }
    }
}
