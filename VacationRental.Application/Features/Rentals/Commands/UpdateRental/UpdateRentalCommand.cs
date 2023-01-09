using System.Windows.Input;
using VacationRental.Application.Contracts.Pipeline;
using VacationRental.Domain.Messages.Rentals;

namespace VacationRental.Application.Features.Rentals.Commands.UpdateRental
{
    public sealed record class UpdateRentalCommand(int Id, int Units, int PreparationTimeInDays) : ICommand<RentalDto>
    {
    }
}
