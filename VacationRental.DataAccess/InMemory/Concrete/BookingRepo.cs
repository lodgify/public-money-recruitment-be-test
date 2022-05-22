using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using VacationRental.DataAccess.InMemory.Abstract;
using VacationRental.Domain.DTOs;
using VacationRental.Domain.Entities;

namespace VacationRental.DataAccess.InMemory.Concrete
{
    public class BookingRepo : IBookingRepo
    {
        private readonly Context context;
        public BookingRepo(Context context)
        {
            this.context = context;
        }
        public Booking Create(Booking booking)
        {
            booking.Id = context.bookings.Keys.Count + 1;
            context.bookings.Add(booking.Id, booking);

            return booking;
        }

        public Booking GetById(int id)
        {
            if (context.bookings.ContainsKey(id))
            {
                return context.bookings[id];
            }
            return null;
        }
        public BookingList GetAll(int rentalId)
        {
            var bookings = new BookingList()
            {
                StartDates = context.bookings.Values
                .Where(b => b.RentalId == rentalId)
                .Select(b => b.Start)
                .ToList(),

                EndDates = context.bookings.Values
                .Where(b => b.RentalId == rentalId)
                .Select(b => b.Start.AddDays(b.Nights))
                .ToList()
            };           

            return bookings;
        }
    }
}
