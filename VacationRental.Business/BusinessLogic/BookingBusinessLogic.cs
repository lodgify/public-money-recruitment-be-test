using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using VacationRental.Api.Models;
using VacationRental.Business.Validators;
using VacationRental.Infrastructure.Models;
using VacationRental.Infrastructure.Repositories;

namespace VacationRental.Business.BusinessLogic
{
    public class BookingBusinessLogic
    {
        private readonly IBookingsRepository _bookingsRepository;
        private readonly IRentalRepository _rentalRepository;
        private readonly IMapper _mapper;
        public BookingBusinessLogic(IBookingsRepository bookingsRepository, IRentalRepository rentalRepository, IMapper mapper)
        {
            _bookingsRepository = bookingsRepository;
            _rentalRepository = rentalRepository;
            _mapper = mapper;
        }

        public BookingViewModel AddBooking(BookingBindingModel model)
        {
            if (!IsPositiveNumberValidator.Validate(model.Nights))
                throw new ApplicationException("Nigts must be positive");
            if (!_rentalRepository.Exists(model.RentalId))
                throw new ApplicationException("Rental not found");

            var rental = _rentalRepository.Get(model.RentalId);
            var conflictingBookings = _bookingsRepository.GetConflictingBookings(model.Start, model.Nights, rental.PreparationTimeInDays, rental.Id).ToArray();

            if (conflictingBookings.Length >= rental.Units)
                throw new ApplicationException("Not available");

            var id = _bookingsRepository.Add(new Booking
            {
                Nights = model.Nights,
                RentalId = model.RentalId,
                Start = model.Start.Date
            });

            var booking = _bookingsRepository.Get(id);
            var bookingVm = _mapper.Map<BookingViewModel>(booking);

            return bookingVm;
        }

        public BookingViewModel GetBooking(int id)
        {
            var booking = _bookingsRepository.Get(id);

            if(booking == null)
                throw new ApplicationException("Booking not found");

            return _mapper.Map<BookingViewModel>(booking);
        }
    }
}
