using System;
using System.Collections.Generic;
using System.Text;
using VacationRental.Business.Abstract;
using VacationRental.Domain.DTOs;
using VacationRental.Domain.Entities;
using VacationRental.Domain.Repositories;

namespace VacationRental.Business.Concrete
{
    public class BookingService : IBookingService
    {
        private readonly IBookingRepository bookingRepository;

        public BookingService(IBookingRepository bookingRepository)
        {
            this.bookingRepository = bookingRepository;
        }

        public Booking Create(Booking booking)
        {
            return bookingRepository.Create(booking);
        }
        public Booking GetById(int id)
        {
            return bookingRepository.GetById(id);
        }

        public BookingList GetAll(int rentalId)
        {
            return bookingRepository.GetAll(rentalId);
        }
    }
}
