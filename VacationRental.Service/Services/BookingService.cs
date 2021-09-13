using VacationRental.Data;
using System.Linq;
using VacationRental.Business;
using System;

namespace VacationRental.Application
{
    public class BookingService : IBookingService
    {
        private IBookingRepository _iBookingRepository;
        private IRentalRepository _iRentalRepository;

        public BookingService(IBookingRepository iBookingRepository, IRentalRepository iRentalRepository)
        {
            _iBookingRepository = iBookingRepository;
            _iRentalRepository = iRentalRepository;
        }

        public AddBookingResponse AddBooking(AddBookingRequest request)
        {
            AddBookingResponse response = new AddBookingResponse();

            try
            { 
            if (request.NumberOfNigths < 0)
            {
                response.Message = "Nights must be positive";
                response.ResourceIdViewModel = new ResourceIdViewModel() { Id = -2 };
                response.Success = false;
                return response;
            }

            var rental = _iRentalRepository.GetById(request.RentalId);
            
            if (rental == null)
            {
                response.Message = "Rental not found";
                response.ResourceIdViewModel = new ResourceIdViewModel() { Id = -2 };
                response.Success = false;
                return response;
            }

            for (var i = 0; i < request.NumberOfNigths; i++)
            {
                var count = 0;
                
                foreach (var booking in rental.BookingCollection)
                {
                    if (   (booking.StartDate <= request.StartDate.Date && booking.StartDate.AddDays(booking.NumberOfNights + rental.PreparationTimeInDays) > request.StartDate.Date)
                        || (booking.StartDate < request.StartDate.AddDays(request.NumberOfNigths + rental.PreparationTimeInDays) && booking.StartDate.AddDays(booking.NumberOfNights + rental.PreparationTimeInDays) >= request.StartDate.AddDays(request.NumberOfNigths + rental.PreparationTimeInDays))
                        || (booking.StartDate > request.StartDate && booking.StartDate.AddDays(booking.NumberOfNights + rental.PreparationTimeInDays) < request.StartDate.AddDays(request.NumberOfNigths + rental.PreparationTimeInDays)))
                    {
                        count++;
                    }
                }
                if (count >= _iRentalRepository.GetById(request.RentalId).Units)
                {
                    response.Message = "Not available";
                    response.ResourceIdViewModel = new ResourceIdViewModel() { Id = -2 };
                    response.Success = false;
                    return response;
                }                   
            }
            
            Booking tmpBooking = new Booking()
            {
                NumberOfNights = request.NumberOfNigths,                
                StartDate = request.StartDate.Date,
                Rental = rental
            };

            _iRentalRepository.AddBooking(tmpBooking);

            response.Success = true;
            response.ResourceIdViewModel = new ResourceIdViewModel() { Id = tmpBooking.Id };

            }
            catch (Exception exception)
            {
                response.Success = false;
                response.Message = exception.Message;
                response.ResourceIdViewModel = new ResourceIdViewModel() { Id = -1};
            }

            return response;
        }

        public GetBookingResponse GetBooking(GetBookingRequest request)
        {
            GetBookingResponse response = new GetBookingResponse();

            try
            { 
            Booking booking = _iBookingRepository.GetAll().Where(w => w.Id == request.bookingId).FirstOrDefault();

            if (booking == null)
            {
                response.Success = false;
                response.Message = "Booking not found";
                return response;
            }

            response.Success = true;
            response.BookingViewModel = booking.ConvertToBookingViewModel();
            }
            catch (Exception exception)
            {
                response.Success = false;
                response.Message = exception.Message;                
            }

            return response;
        }
    }    
}
