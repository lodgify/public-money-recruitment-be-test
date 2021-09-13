using VacationRental.Data;
using System.Linq;
using VacationRental.Business;
using System;

namespace VacationRental.Application
{
    public class RentalService : IRentalService
    {
        private IBookingRepository _iBookingRepository;
        private IRentalRepository _iRentalRepository;

        public RentalService(IBookingRepository iBookingRepository, IRentalRepository iRentalRepository)
        {
            _iBookingRepository = iBookingRepository;
            _iRentalRepository = iRentalRepository;
        }
        
        public UpdateRentalResponse UpdateRental(UpdateRentalRequest request)
        {
            UpdateRentalResponse response = new UpdateRentalResponse();

            Rental rental = _iRentalRepository.GetById(request.Id);

            // If the request is less restrictive we can update the rental
            if (rental.BookingCollection.Count() == 0 && (rental.PreparationTimeInDays == request.PreparationTimeInDays && rental.Units < request.Units))
            {
                rental.PreparationTimeInDays = request.PreparationTimeInDays;
                rental.Units = request.Units;
                response.Success = true;
                return response;
            }

            // The request should fail if overlapping between existing bookings and/ or their preparation times occurs due to a decrease of the number of units or an increase of the length of preparation time.
            // If the length of preparation time is changed then it should be updated for all existing bookings
            foreach (Booking boooking in rental.BookingCollection)
            {                 
                // TODO, Sorry, This week I have no time to do more              
            }    

            return response;
        }

        public GetRentalResponse GetRental(GetRentalRequest request)
        {
            GetRentalResponse response = new GetRentalResponse();

            try
            {
                var rental = _iRentalRepository.GetById(request.rentalId);

                if (rental == null)
                {
                    response.Message = "Rental not found";
                    response.Success = false;
                    return response;
                }

                response.Success = true;
                response.RentalViewModel = rental.ConvertToRentalViewModel();

            }
            catch (Exception exception)
            {
                response.Success = false;
                response.Message = exception.Message;                
            }
            
            return response;
        }
        public AddRentalResponse AddRental(AddRentalRequest request)
        {
            AddRentalResponse response = new AddRentalResponse();

            try
            {
                Rental tmpRental = new Rental()
                {
                    PreparationTimeInDays = request.PreparationTimeInDays,
                    RentalType = RentalTypeEnum.Unknown,
                    Units = request.Units
                };

                _iRentalRepository.Add(tmpRental);

                response.Success = true;
                response.ResourceIdViewModel = new ResourceIdViewModel() { Id = tmpRental.Id };
            }
            catch (Exception exception)
            {
                response.Success = false;
                response.Message = exception.Message;
                response.ResourceIdViewModel = new ResourceIdViewModel() { Id = -1 };
            }
            
            return response;
        }
    }
    
}
