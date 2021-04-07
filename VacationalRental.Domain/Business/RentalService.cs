using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VacationalRental.Domain.Entities;
using VacationalRental.Domain.Enums;
using VacationalRental.Domain.Interfaces.Repositories;
using VacationalRental.Domain.Interfaces.Services;

namespace VacationalRental.Domain.Business
{
    public class RentalService : IRentalService
    {
        public readonly IRentalsRepository _rentalsRepository;

        public RentalService(IRentalsRepository rentalsRepository)
        {
            _rentalsRepository = rentalsRepository;
        }

        public async Task<(InsertNewRentalStatus, int)> InsertNewRentalObtainRentalId(RentalEntity rentalEntity)
        {
            //if (rentalEntity.PreprationTimeInDays > rentalEntity.Units)
            //    return (InsertNewRentalStatus.PreparationDaysHigherThanUnits, 0);

            var rentalId = await _rentalsRepository.InsertNewRentalObtainRentalId(rentalEntity);

            if (rentalId <= 0)
                return (InsertNewRentalStatus.InsertDbNoRowsAffected, rentalId);

            return (InsertNewRentalStatus.OK, rentalId);
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
    }
}
