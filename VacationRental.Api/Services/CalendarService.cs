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
        private readonly IMapper _mapper;
        private readonly IValidator<ReserveBindingModel> _validator;

        public CalendarService(
            IBookingRepository bookingRepository,
            IMapper mapper,
            IValidator<ReserveBindingModel> validator
        )
        {
            _bookingRepository = bookingRepository;
            _mapper = mapper;
            _validator = validator;
        }

        public async Task<CalendarViewModel> GetCalendar(ReserveBindingModel model)
        {
            var validationResult = await _validator.ValidateAsync(model);

            if (!validationResult.IsValid)
                throw new ApplicationException(validationResult.Errors.First().ErrorMessage);

            var calendar = new CalendarViewModel { RentalId = model.RentalId };

            for (int i = 0; i < model.Nights; i++)
            {
                var date = new CalendarDateViewModel { Date = model.Start.Date.AddDays(i) };

                date.Bookings = AddBookings(model.RentalId, date.Date);

                calendar.Dates.Add(date);
            }

            return calendar;
        }

        private List<CalendarBookingViewModel> AddBookings(int rentalId, DateTime date)
        {
            var CalenderBookingList = new List<CalendarBookingViewModel>();

            foreach (var booking in _bookingRepository.GetAll())
            {
             if (booking.RentalId == rentalId
                        && booking.Start <= date.Date && booking.Start.AddDays(booking.Nights) > date.Date)
                    CalenderBookingList.Add(new CalendarBookingViewModel { Id = booking.Id });
            }

            return CalenderBookingList;
        }
    }
}
