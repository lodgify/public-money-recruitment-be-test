using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VacationalRental.Domain.Entities;
using VacationalRental.Domain.Enums;

namespace VacationalRental.Domain.Interfaces.Services
{
    public interface IRentalService
    {
        Task<int> InsertNewRentalObtainRentalId(RentalEntity rentalEntity);

        Task<RentalEntity> GetRentalById(int rentalId);

        Task<bool> RentalExists(int rentalId);
    }
}
