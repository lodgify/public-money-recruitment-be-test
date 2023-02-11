﻿using System;
using System.Collections.Generic;
using VacationRental.Api.Models;
using VacationRental.Application.Contracts.Persistence;
using VacationRental.Application.Contracts.Pipeline;
using VacationRental.Application.Exceptions;
using VacationRental.Domain.Aggregates.Calendars;
using VacationRental.Domain.Errors;
using VacationRental.Domain.Messages.Calendars;
using VacationRental.Domain.Models.Rentals;

namespace VacationRental.Application.Features.Calendars.Queries.GetRentalCalendar
{
    public class GetRentalCalendarQueryHandler : IQueryHandler<GetRentalCalendarQuery, CalendarDto>
    {
        private readonly IRepository<Rental> _rentalRepository;
        private readonly IBookingRepository _bookingRepository;        

        public GetRentalCalendarQueryHandler(IRepository<Rental> rentalRepository, IBookingRepository bookingRepository)
        {
            _rentalRepository = rentalRepository;
            _bookingRepository = bookingRepository;            
        }

        public CalendarDto Handle(GetRentalCalendarQuery request)
        {            
            var rental = _rentalRepository.GetById(request.RentalId);
            if (rental == null)
                throw new NotFoundException(RentalError.RentalNotFound);

            var result = new CalendarDto
            {
                RentalId = rental.Id,
                Dates = new List<CalendarDate>()
            };

            var bookings = _bookingRepository.GetBookingByRentalId(request.RentalId);            
            for (var i = 0; i < request.Nights; i++)
            {
                var date = new CalendarDate(request.Start.Date.AddDays(i));
                
                foreach (var booking in bookings)
                {
                    date.AggregateBookings(booking);
                    date.DefinePreparationTimes(booking, rental.PreparationTimeInDays);                    
                }

                result.Dates.Add(date);
            }

            return result;
        }                
    }
}
