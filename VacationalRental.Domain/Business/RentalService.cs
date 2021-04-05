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

        public async Task<int> InsertNewRentalObtainRentalId(RentalEntity rentalEntity)
        {
            return await _rentalsRepository.InsertNewRentalObtainRentalId(rentalEntity);
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
