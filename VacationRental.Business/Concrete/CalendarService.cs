using System;
using System.Collections.Generic;
using System.Text;
using VacationRental.Business.Abstract;
using VacationRental.Domain.DTOs;
using VacationRental.Domain.Entities;
using VacationRental.Domain.Repositories;

namespace VacationRental.Business.Concrete
{
    public class CalendarService : ICalendarService
    {
        private readonly ICalendarRepository calendarRepository;

        public CalendarService(ICalendarRepository calendarRepository)
        {
            this.calendarRepository = calendarRepository;
        }

        public List<List<KeyValuePair<int, Booking>>> GetBookings(CalendarDto dto)
        {
           return calendarRepository.GetBookings(dto);
        }
    }
}
