using RentalSoftware.Core.Contracts.Request;
using RentalSoftware.Core.Contracts.Response;
using RentalSoftware.Core.Entities;
using RentalSoftware.Core.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace RentalSoftware.Core.Services
{
    public class BookingService : IBookingService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IRentalService _rentalService;

        public BookingService(IUnitOfWork unitOfWork, IRentalService rentalService)
        {
            _unitOfWork = unitOfWork;
            _rentalService = rentalService;
        }

        public async Task<AddBookingResponse> AddBooking(AddBookingRequest request)
        {
            AddBookingResponse response = new AddBookingResponse();

            try
            {
                if (request.Nights < 0)
                {
                    response.Message = "Nights must be positive";
                    response.ResourceIdViewModel = new IdentifierViewModel() { Id = -2 };
                    response.Succeeded = false;
                    return response;
                }

                var rental = await _rentalService.GetByRentalId(request.RentalId);

                if (rental == null)
                {
                    response.Message = "Rental not found";
                    response.ResourceIdViewModel = new IdentifierViewModel() { Id = -2 };
                    response.Succeeded = false;
                    return response;
                }

                for (var i = 0; i < request.Nights; i++)
                {
                    var count = 0;

                    foreach (var bookingItem in rental.Bookings)
                    {
                        if ((bookingItem.Start <= request.StartDate.Date && bookingItem.Start.AddDays(bookingItem.Nights + rental.PreparationTime) > request.StartDate.Date)
                            || (bookingItem.Start < request.StartDate.AddDays(request.Nights + rental.PreparationTime) && bookingItem.Start.AddDays(bookingItem.Nights + rental.PreparationTime) >= request.StartDate.AddDays(request.Nights + rental.PreparationTime))
                            || (bookingItem.Start > request.StartDate && bookingItem.Start.AddDays(bookingItem.Nights + rental.PreparationTime) < request.StartDate.AddDays(request.Nights + rental.PreparationTime)))
                            {
                                count++;
                            }
                    }

                    var rentalUnits = rental.Units;

                    if (count >= rentalUnits)
                    {
                        response.Message = "Not Available";
                        response.ResourceIdViewModel = new IdentifierViewModel() { Id = -2 };
                        response.Succeeded = false;
                        return response;
                    }
                }

                Booking booking = new Booking()
                {
                    Nights = request.Nights,
                    Start = request.StartDate.Date,
                    RentalId = request.RentalId
                };

                await _unitOfWork.BookingRepository.AddAsync(booking);
                await _unitOfWork.Complete();

                response.Succeeded = true;
                response.ResourceIdViewModel = new IdentifierViewModel() { Id = booking.Id };

            }
            catch (Exception ex)
            {
                response.Succeeded = false;
                response.Message = ex.Message;
                response.ResourceIdViewModel = new IdentifierViewModel() { Id = -1 };
            }

            return response;
        }

        public async Task<GetBookingResponse> GetBooking(GetBookingRequest request)
        {
            GetBookingResponse response = new GetBookingResponse();

            try
            {
                var booking = await _unitOfWork.BookingRepository.GetByIdAsync(request.BookingId);

                if (booking == null)
                {
                    response.Succeeded = false;
                    response.Message = "Booking not found";
                    return response;
                }

                response.Succeeded = true;

                BookingViewModel bookingViewModel = new BookingViewModel
                {
                    Id = booking.Id,
                    Nights = booking.Nights,
                    RentalId = booking.RentalId,
                    Start = booking.Start
                };

                response.BookingViewModel = bookingViewModel;
            }
            catch (Exception ex)
            {
                response.Succeeded = false;
                response.Message = ex.Message;
            }

            return response;
        }

        public async Task<IEnumerable<Booking>> GetAll()
        {
            return await _unitOfWork.BookingRepository.GetAll();
        }
    }
}
