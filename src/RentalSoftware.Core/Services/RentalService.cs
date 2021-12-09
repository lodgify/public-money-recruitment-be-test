using RentalSoftware.Core.Contracts.Request;
using RentalSoftware.Core.Contracts.Response;
using RentalSoftware.Core.Entities;
using RentalSoftware.Core.Enums;
using RentalSoftware.Core.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RentalSoftware.Core.Services
{
    public class RentalService : IRentalService
    {
        private readonly IUnitOfWork _unitOfWork;

        public RentalService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;

        }

        public async Task<Rental> GetByRentalId(int rentalId)
        {
            var rental = await _unitOfWork.RentalRepository.GetRentalById(rentalId);

            var bookings = await _unitOfWork.BookingRepository.GetAll();

            rental.Bookings = bookings.Where(x => x.RentalId == rentalId).ToList();

            return rental;
        }

        public async Task<List<Rental>> GetAll()
        {
            return await _unitOfWork.RentalRepository.GetAll();
        }

        public async Task<GetRentalResponse> GetRental(GetRentalRequest request)
        {
            GetRentalResponse response = new GetRentalResponse();

            try
            {
                var rental = await _unitOfWork.RentalRepository.Get(request.RentalId);

                if (rental == null)
                {
                    response.Message = "Rental not found";
                    response.Succeeded = false;
                    return response;
                }

                response.Succeeded = true;

                RentalViewModel rentalViewModel = new RentalViewModel
                {
                    Id = rental.Id,
                    PreparationTime = rental.PreparationTime,
                    Units = rental.Units
                };

                response.RentalViewModel = rentalViewModel;

            }
            catch (Exception ex)
            {
                response.Succeeded = false;
                response.Message = ex.Message;
            }

            return response;
        }
        public async Task<AddRentalResponse> AddRental(AddRentalRequest request)
        {
            AddRentalResponse response = new AddRentalResponse();

            try
            {
                Rental tmpRental = new Rental()
                {
                    PreparationTime = request.PreparationTime,
                    RentalType = RentalType.Other,
                    Units = request.Units
                };

                await _unitOfWork.RentalRepository.AddAsync(tmpRental);
                await _unitOfWork.Complete();

                response.Succeeded = true;
                response.ResourceIdViewModel = new IdentifierViewModel() { Id = tmpRental.Id };
            }
            catch (Exception ex)
            {
                response.Succeeded = false;
                response.Message = ex.Message;
                response.ResourceIdViewModel = new IdentifierViewModel() { Id = -1 };
            }

            return response;
        }

        public async Task<UpdateRentalResponse> UpdateRental(UpdateRentalRequest request)
        {
            UpdateRentalResponse response = new UpdateRentalResponse();

            var rental = await _unitOfWork.RentalRepository.Get(request.Id);

            ////3. * *Optional * *(nice to have). Our customers requested the possibility of updating their existing rentals. They would like to be able to change the number
            ////                                  of units and the length of preparation time. Please provide a new endpoint which allows them to do it:

            /// Come back to this if I have time.           

            return response;
        }
    }
}
