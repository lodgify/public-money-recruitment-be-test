using System;
using System.Collections.Generic;
using System.Text;
using VacationRental.Domain.DTOs;
using VacationRental.Domain.Entities;

namespace VacationRental.Business.Abstract
{
    public interface IBookingService
    {
        Booking Create(Booking booking);
        Booking GetById(int id);
        BookingList GetAll(int rentalId);
    }
}
