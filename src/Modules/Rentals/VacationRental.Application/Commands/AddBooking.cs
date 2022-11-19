using System;
using VacationRental.Shared.Abstractions.Commands;

namespace VacationRental.Application.Commands
{
    internal record AddBooking(int RentalId, DateTime Start, int Nights) : ICommand<int>
    {
    }
}
