using System;
using System.Collections.Generic;
using System.Linq;
using VacationRental.Booking.Domain.Interfaces;
using VacationRental.Booking.Entities.Rentals.Events;
using VacationRental.Domain.Interfaces;
using VacationRental.Rental.Domain.Interfaces;

namespace VacationRental.Booking.Domain
{
    public partial class Booking : IAggregateRoot
    {
        public Booking(int rentalId, int unit, int preparationTime, DateTime startDate, int nights)
        {
            this.Update(rentalId, unit, preparationTime, startDate, nights, null, false);
        }

        public Booking(Booking booking)
        {
            this.Update(booking);
            this.Update(booking.Guid);
        }

        public override object HandleClone()
        {
            return new Booking(this);

        }

        internal static Booking Create(IBooking booking, List<IBooking> bookings, IRentalEntity rental)
        {
            return Booking.Create(rental, bookings, booking.Id, booking.Unit, booking.Start, booking.Nights);
        }

        public Booking Validate(List<IBooking> currentRentalBookings, IRental rental)
        {
            int realNightsBlocked = rental.PreparationTimeInDays + this.Nights;
            var selectedUnit = 1;

            for (var i = 0; i < realNightsBlocked; i++)
            {
                var count = selectedUnit;
                foreach (var booking in currentRentalBookings.Where(x => x.RentalId == rental.Id && x.Unit == selectedUnit))
                {
                    int bookingNightsBlocked = rental.PreparationTimeInDays + booking.Nights;
                    if ((booking.Start <= this.Start.Date && booking.Start.AddDays(bookingNightsBlocked) > this.Start.Date)
                         || (booking.Start < this.Start.AddDays(realNightsBlocked) && booking.Start.AddDays(bookingNightsBlocked) >= this.Start.AddDays(realNightsBlocked))
                         || (booking.Start > this.Start && booking.Start.AddDays(bookingNightsBlocked) < this.Start.AddDays(realNightsBlocked)))
                    {
                        count++;
                    }
                }
                if (count > rental.Units)
                    throw new ApplicationException("Not available");
                selectedUnit = count;
            }
            this.Unit = selectedUnit;
            return this;
        }

        public static Booking Create(IRentalEntity rental, List<IBooking> bookings, int bookingId, int unit, DateTime startDate, int nights)
        {
            Booking booking = new Booking(((IRentalId)rental).Id, unit, rental.PreparationTimeInDays, startDate, nights);
            booking.Validate(bookings, rental);
            booking.AddEvent(new OnBookingCreatedDomainEvent(booking, rental));
            return booking;
        }


        internal void Update(IBooking request)
        {
            this.Update(request.RentalId, request.Unit, request.PreparationTime, request.Start, request.Nights, null, true);
        }

        public void Update(int rentalId, int unit, int preparationTime, DateTime startDate, int nights, Guid? guid, bool addEvent = false)
        {
            this.Guid = guid ?? this.Guid;

            this.Unit = unit;
            this.Nights = nights;
            this.Start = startDate;
            this.RentalId = rentalId;

            this.PreparationTime = preparationTime;
            if (addEvent)
            {
                AddEvent(new OnBookingUpdatedDomainEvent(this));
            }
        }
    }
}