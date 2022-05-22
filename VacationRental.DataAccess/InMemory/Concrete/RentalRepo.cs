using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using VacationRental.Api.Utilities;
using VacationRental.Business.Mapper;
using VacationRental.DataAccess.InMemory.Abstract;
using VacationRental.Domain.DTOs;
using VacationRental.Domain.Entities;

namespace VacationRental.DataAccess.InMemory.Concrete
{
    public class RentalRepo : IRentalRepo
    {
        private readonly Context context;
        public RentalRepo(Context context)
        {
            this.context = context;
        }
        public Rental Create(Rental rental)
        {
            rental.Id = context.rentals.Keys.Count + 1;
            context.rentals.Add(rental.Id, rental);

            return rental;
        }

        public Rental GetById(int id)
        {
            if (context.rentals.ContainsKey(id))
            {
                return context.rentals[id];
            }
            return null;
        }

        public Rental Update(Rental rental, RentalDto dto)
        {
          
            var bookings = new BookingList()
            {
                StartDates = context.bookings.Values
                .Where(b => b.RentalId == rental.Id)
                .Select(b => b.Start)
                .ToList(),

                EndDates = context.bookings.Values
                .Where(b => b.RentalId == rental.Id)
                .Select(b => b.Start.AddDays(b.Nights + dto.PreparationTimeInDays - rental.PreparationTimeInDays))
                .ToList()
            };

            if (dto.Units < rental.Units)
            {
                int bookingNumbers = BookingHandler.CheckBookings(bookings.StartDates, bookings.EndDates, dto.Units);

                if (bookingNumbers > dto.Units)
                {
                    return null;
                }
            }

            if (dto.PreparationTimeInDays > rental.PreparationTimeInDays)
            {
                int bookingNumbers = BookingHandler.CheckBookings(bookings.StartDates, bookings.EndDates, dto.Units);

                if (bookingNumbers > dto.Units)
                {
                    return null;
                }
            }

            for (int i = 1; i <= context.bookings.Count; i++)
            {
                context.bookings[i].Nights += dto.PreparationTimeInDays - rental.PreparationTimeInDays;
            }

            context.rentals[rental.Id].Units = dto.Units;
            context.rentals[rental.Id].PreparationTimeInDays = dto.PreparationTimeInDays;

            return rental;
        }
    }
}
