using VacationRental.Models.Dtos;

namespace VacationRental.Services.Interfaces
{
    public interface IAccountService
    {
        Task<AccessTokenDto> SignInGuestAsync();
    }
}
