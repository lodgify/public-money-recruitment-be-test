using AutoMapper;
using System;
using System.Collections.Generic;
using System.Text;
using VacationRental.Api.Models;
using VacationRental.Business.Validators;
using VacationRental.Infrastructure.Models;
using VacationRental.Infrastructure.Repositories;

namespace VacationRental.Business.BusinessLogic
{
    public class RentalBusinessLogic
    {
        private readonly IRentalRepository _rentalRepository;
        private readonly IBookingsRepository _bookingsRepository;
        private readonly BookingBusinessLogic _bookingBusinessLogic;
        private IMapper _mapper;

        public RentalBusinessLogic(IRentalRepository rentalRepository, IBookingsRepository bookingsRepository, BookingBusinessLogic bookingBusinessLogic, IMapper mapper)
        {
            _rentalRepository = rentalRepository;
            _mapper = mapper;
            _bookingsRepository = bookingsRepository;
            _bookingBusinessLogic = bookingBusinessLogic;
        }

        public RentalViewModel GetRental(int id)
        {
            if(!_rentalRepository.Exists(id))
                throw new ApplicationException("Rental not found");

            var rental = _rentalRepository.Get(id);
            return _mapper.Map<RentalViewModel>(rental);
        }

        public int AddRental(RentalBindingModel model)
        {
            var rental = _mapper.Map<Rental>(model);
            int id = _rentalRepository.Add(rental);
            return id;
        }
        
        public RentalViewModel UpdateRental(int id, RentalBindingModel model)
        {
            if (!IsPositiveNumberValidator.Validate(model.Units))
                throw new ApplicationException("Units must be positive");
            if (!_rentalRepository.Exists(id))
                throw new ApplicationException("Rental not found");

            var oldRental = _rentalRepository.Get(id);
            oldRental = new Rental
            {
                Id = oldRental.Id,
                PreparationTimeInDays = oldRental.PreparationTimeInDays,
                Units = oldRental.Units,
            };

            var newRental = _mapper.Map<Rental>(model);
            newRental.Id = id;

            _rentalRepository.Update(newRental);

            var allBookings = new List<Booking>(_bookingsRepository.GetAll((booking) => true));
            _bookingsRepository.DeleteAll((booking) => true);

            try
            {
                foreach (var booking in allBookings)
                    _bookingBusinessLogic.AddBooking(_mapper.Map<BookingBindingModel>(booking));
            }
            catch(Exception ex)
            {
                _rentalRepository.Update(oldRental);
                _bookingsRepository.DeleteAll((booking) => true);

                foreach (var booking in allBookings)
                    _bookingBusinessLogic.AddBooking(_mapper.Map<BookingBindingModel>(booking));

                throw ex;
            }

            return _mapper.Map<RentalViewModel>(newRental);
        }
    }
}
