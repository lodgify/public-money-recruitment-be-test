using System.Threading.Tasks;
using VacationalRental.Domain.Entities;
using VacationalRental.Domain.Enums;
using VacationalRental.Domain.Models;

namespace VacationalRental.Domain.Interfaces.Services
{
    public interface IRentalService
    {
        Task<(InsertUpdateNewRentalStatus, int)> InsertNewRentalObtainRentalId(RentalEntity rentalEntity);

        Task<int> GetRentalPreparationTimeInDays(int rentalId);

        Task<RentalEntity> GetRentalById(int rentalId);

        Task<bool> RentalExists(int rentalId);

        Task<(InsertUpdateNewRentalStatus, VacationalRentalModel)> UpdateRental(VacationalRentalModel vacationalRentalModel);
    }
}
