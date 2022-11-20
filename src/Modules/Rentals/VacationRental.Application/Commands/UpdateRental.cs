using VacationRental.Shared.Abstractions.Commands;

namespace VacationRental.Application.Commands
{
    internal record UpdateRental(int RentalId, int Units, int PreparationTimeInDays) : ICommand<int>
    {
    }
}
