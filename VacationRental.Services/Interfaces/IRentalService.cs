using VacationRental.Models.Dtos;
using VacationRental.Models.Paramaters;

namespace VacationRental.Services.Interfaces
{
    public interface IRentalService
    {
        Task<IEnumerable<RentalDto>> GetRentalsAsync();
        Task<RentalDto> GetRentalByIdAsync(int rentalId);
        Task<BaseEntityDto> AddRentalAsync(RentalParameters parameters);
        Task UpdateRentalAsync(int rentalId, RentalParameters parameters);
        Task DeleteRentalAsync(int rentalId);
    }
}
