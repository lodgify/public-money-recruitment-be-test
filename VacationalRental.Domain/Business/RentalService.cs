using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VacationalRental.Domain.Entities;
using VacationalRental.Domain.Enums;
using VacationalRental.Domain.Interfaces.Repositories;
using VacationalRental.Domain.Interfaces.Services;
using VacationalRental.Domain.Models;

namespace VacationalRental.Domain.Business
{
    public class RentalService : IRentalService
    {
        public readonly IRentalsRepository _rentalsRepository;
        public readonly IBookingsRepository _bookingRepository;

        public RentalService(
            IRentalsRepository rentalsRepository,
            IBookingsRepository bookingRepository)
        {
            _rentalsRepository = rentalsRepository;
            _bookingRepository = bookingRepository;
        }

        public async Task<(InsertUpdateNewRentalStatus, int)> InsertNewRentalObtainRentalId(RentalEntity rentalEntity)
        {
            //if (rentalEntity.PreprationTimeInDays > rentalEntity.Units)
            //    return (InsertNewRentalStatus.PreparationDaysHigherThanUnits, 0);

            var rentalId = await _rentalsRepository.InsertNewRentalObtainRentalId(rentalEntity);

            if (rentalId <= 0)
                return (InsertUpdateNewRentalStatus.InsertUpdateDbNoRowsAffected, rentalId);

            return (InsertUpdateNewRentalStatus.OK, rentalId);
        }

        public async Task<int> GetRentalPreparationTimeInDays(int rentalId)
        {
            return await _rentalsRepository.GetRentalPreparationTimeInDays(rentalId);
        }

        public async Task<RentalEntity> GetRentalById(int rentalId)
        {
            return await _rentalsRepository.GetRentalById(rentalId);
        }

        public async Task<bool> RentalExists(int rentalId)
        {
            return await _rentalsRepository.RentalExists(rentalId);
        }

        public async Task<(InsertUpdateNewRentalStatus, VacationalRentalModel)> UpdateRental(VacationalRentalModel vacationalRentalModel)
        {
            if (!await RentalExists(vacationalRentalModel.RentalId))
                return (InsertUpdateNewRentalStatus.RentalNotExists, null); 

            var bookings = await _bookingRepository.GetBookinByRentalId(vacationalRentalModel.RentalId);
            var rentalEntity = await _rentalsRepository.GetRentalById(vacationalRentalModel.RentalId);

            var currentUnitsBooked = 0;
            foreach (var booking in bookings)
            {
                var lastDateTimeBooking = booking.Start.AddDays(booking.Nights + rentalEntity.PreprationTimeInDays);

                if (lastDateTimeBooking >= DateTime.Now.Date)
                    currentUnitsBooked++;
            }

            if (currentUnitsBooked > vacationalRentalModel.Units)
                return (InsertUpdateNewRentalStatus.UnitsQuantityBookedAlready, new VacationalRentalModel
                {
                    UnitsBooked = currentUnitsBooked
                });

            var listDates = new List<BookingDateStartEndModel>();
            foreach (var booking in bookings)
            {
                listDates.Add(new BookingDateStartEndModel
                {
                    Start = booking.Start,
                    End = booking.Start.AddDays(booking.Nights + rentalEntity.PreprationTimeInDays)
                }); ;
            }

            var datesOverlapping = false;
            for (int date = 0; date < listDates.Count; date++)
            {
                for (int dateToCheck = 0; dateToCheck < listDates.Count; dateToCheck++)
                {
                    if (date == dateToCheck)
                        continue;

                    if (listDates[date].Start <= listDates[dateToCheck].End && listDates[dateToCheck].Start <= listDates[date].End)
                    {
                        datesOverlapping = true;
                        break;
                    }
                }
            }

            if (datesOverlapping)
                return (InsertUpdateNewRentalStatus.DatesOverlapping, null);

            var rowsAffected = await _rentalsRepository.UpdateRental(
                new RentalEntity
                {
                    Id = vacationalRentalModel.RentalId,
                    PreprationTimeInDays = vacationalRentalModel.PreparationTimeInDays,
                    Units = vacationalRentalModel.Units
                });

            var rentalEntityResult = await GetRentalById(vacationalRentalModel.RentalId);
            var vacationRentalModelResult = new VacationalRentalModel
            {
                PreparationTimeInDays = rentalEntityResult.PreprationTimeInDays,
                RentalId = rentalEntityResult.Id,
                Units = rentalEntityResult.Units
            };

            if (rowsAffected <= 0)
                return (InsertUpdateNewRentalStatus.InsertUpdateDbNoRowsAffected, vacationRentalModelResult);

            return (InsertUpdateNewRentalStatus.OK, vacationRentalModelResult);
        }
    }
}
