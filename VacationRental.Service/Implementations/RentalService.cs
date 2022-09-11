using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using VacationRental.Common.Models;
using VacationRental.Data.Entities;
using VacationRental.Repository.Interfaces;
using VacationRental.Service.Interfaces;

namespace VacationRental.Service.Implementations
{
    public class RentalService : BaseService<IBaseRepository<Rental>, RentalViewModel, Rental>, IRentalService
    {
        private readonly IBookingService _bookingService;
        public RentalService(IMapper mapper, IRentalRepository repository, IBookingService bookingService) : base(mapper, repository)
        {
            _bookingService = bookingService;
        }

        public ResourceIdViewModel CreateRental(RentalViewModel model)
        {
            var item = Add(model);

            var key = new ResourceIdViewModel { Id = item.Id };

            return key;
        }

        public bool UpdateRental(int rentalId, ref RentalViewModel model)
        {
            var rental = Get(rentalId);
            if (rental is null)
                throw new ApplicationException("Rental not found");

            var bookings = _bookingService.GetAll();

            if (rental.Units != model.Units ||
                rental.PreparationTimeInDays != model.PreparationTimeInDays && _bookingService.CheckAvailability(bookings, rental))
            {
                rental.Units = model.Units;
                rental.PreparationTimeInDays = model.PreparationTimeInDays;
                model = AddOrUpdate(rental);
                return true;
            }
            return false;
        }
    }
}
