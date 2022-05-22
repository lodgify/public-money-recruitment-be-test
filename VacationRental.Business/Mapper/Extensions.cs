using System;
using System.Collections.Generic;
using System.Text;
using VacationRental.Domain.DTOs;
using VacationRental.Domain.Entities;

namespace VacationRental.Business.Mapper
{
    public static class Extensions
    {
        public static Rental ToDb(this RentalDto dto)
        {
            var rental = new Rental()
            {
                Units = dto.Units,
                PreparationTimeInDays = dto.PreparationTimeInDays
            };
            return rental;
        }

        public static RentalDto ToBl(this Rental entity)
        {
            return new RentalDto()
            {
                Units = entity.Units,
                PreparationTimeInDays = entity.PreparationTimeInDays
            };
        }

        public static BookingDto ToBl(this Booking booking, Rental rental)
        {
            return new BookingDto()
            {
                RentalId = booking.RentalId,
                Unit = booking.Unit,
                Nights = booking.Nights - rental.PreparationTimeInDays,
                Start = booking.Start
            };
        }
        public static Booking ToDb(this BookingDto dto, Rental rental)
        {
            var booking = new Booking()
            {
                RentalId = dto.RentalId,
                Unit = dto.Unit,
                Nights = dto.Nights + rental.PreparationTimeInDays,
                Start = dto.Start
            };
            return booking;
        }
    }
}
