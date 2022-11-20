using System;

namespace VacationRental.Core.Entities
{
    internal class Booking
    {
        public int Id { get; private set; }

        public DateTime Start { get; private set; }

        public int Nights { get; private set; }

        public int Unit { get; private set; }

        public int RentalId { get; private set; }

        public Rental Rental { get; private set; }

        private Booking()
        {
        }

        public Booking(Rental rental, DateTime start, int nights, int units)
        {
            Rental = rental;
            Start = start.Date;
            Nights = nights;
            Unit = units;
        }

        public void SetBookingId(int bookingId)
        {
            Id = bookingId;
        }

        public bool IsNotAvailable(DateTime date, int specifiedNights)
        {
            var totalBooked = specifiedNights + Rental.PreparationTimeInDays;

            return (Start <= date.Date && PreparationEndDate > date.Date)
                || (Start < date.AddDays(totalBooked) && PreparationEndDate >= date.AddDays(totalBooked))
                || (Start > date && PreparationEndDate < date.AddDays(totalBooked));
        }

        public BookingStatus GetStatus(DateTime date)
        {
            if (IsBooked(date)) return BookingStatus.Booked;
            else if (IsPreparationPeriod(date)) return BookingStatus.Preparation;

            return BookingStatus.Free;
        }

        private bool IsBooked(DateTime date)
        {
            return Start <= date.Date && BookingEndDate > date.Date;
        }

        private bool IsPreparationPeriod(DateTime date)
        {
            return PreparationStartDate <= date.Date && PreparationEndDate > date.Date;
        }

        private DateTime BookingEndDate => Start.AddDays(Nights);

        private DateTime PreparationStartDate => BookingEndDate;

        private DateTime PreparationEndDate => Start.AddDays(Nights + Rental.PreparationTimeInDays);
    }
}
