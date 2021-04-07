using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VacationalRental.Domain.Entities;

namespace VacationalRental.Domain.Interfaces.Repositories
{
    public interface IRentalsRepository
    {
        Task<int> GetRentalUnits(int rentalID);

        Task<int> GetRentalPreparationTimeInDays(int rentalID);

        Task<int> InsertNewRentalObtainRentalId(RentalEntity rentalEntity);

        Task<RentalEntity> GetRentalById(int rentalId);

        Task<bool> RentalExists(int rentalId);
    }
}
