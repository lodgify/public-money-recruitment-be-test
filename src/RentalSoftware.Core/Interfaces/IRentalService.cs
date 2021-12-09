using RentalSoftware.Core.Contracts.Request;
using RentalSoftware.Core.Contracts.Response;
using RentalSoftware.Core.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace RentalSoftware.Core.Interfaces
{
    public interface IRentalService
    {
        Task<List<Rental>> GetAll();
        Task<Rental> GetByRentalId(int rentalId);
        Task<AddRentalResponse> AddRental(AddRentalRequest request);
        Task<GetRentalResponse> GetRental(GetRentalRequest request);
        Task<UpdateRentalResponse> UpdateRental(UpdateRentalRequest request);
    }
}
