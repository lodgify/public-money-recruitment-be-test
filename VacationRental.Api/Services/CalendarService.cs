using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using FluentValidation;
using VacationRental.Api.Contracts.Request;
using VacationRental.Api.Interfaces;
using VacationRental.Api.Models;
using VacationRental.Api.Repository;

namespace VacationRental.Api.Services
{
    public class CalendarService : ICalendarService
    {
        private readonly IBookingRepository _bookingRepository;
        private readonly IRentalRepository _rentalRepository;
        private readonly IMapper _mapper;
        private readonly IValidator<ReserveBindingModel> _validator;

        public CalendarService(
            IBookingRepository bookingRepository,
            IMapper mapper,
            IValidator<ReserveBindingModel> validator,
            IRentalRepository rentalRepository
        )
        {
            _bookingRepository = bookingRepository;
            _mapper = mapper;
            _validator = validator;
            _rentalRepository = rentalRepository;
        }

        public async Task<CalendarViewModel> GetCalendar(ReserveBindingModel model)
        {
            var validationResult = await _validator.ValidateAsync(model);

            if (!validationResult.IsValid)
                throw new ApplicationException(validationResult.Errors.First().ErrorMessage);

            var rental = _rentalRepository.Get(model.RentalId);

            List<DateTime> postPreparationTime = new();
            
           
            if (rental.PreparationTimeInDays > 0)
            {
                postPreparationTime = _bookingRepository.GetPreparationTimes(
                    model.RentalId,
                    model.Start,
                    model.Start.Date.AddDays(model.Nights),
                    rental.PreparationTimeInDays
                );
            }

            var calendar = new CalendarViewModel { RentalId = model.RentalId };

            for (int i = 0; i < model.Nights; i++)
            {
                var date = new CalendarDateViewModel { Date = model.Start.Date.AddDays(i) };

                var booking = _bookingRepository.GetBooking(model.RentalId, date.Date);

                if (booking is not null)
                {
                    date.Bookings = new List<CalendarBookingViewModel>()
                    {
                        new() { Id = booking.Id, Unit = rental.Units }
                    };
                }

                if (postPreparationTime.Any(pt => pt == date.Date))
                {
                    date.PreparationTimes = new List<CalendarPreparationTimeViewModel>()
                    {
                        new() { Unit = rental.Units }
                    };
                }

                calendar.Dates.Add(date);
            }

            return calendar;
        }
    }
}
