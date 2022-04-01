using MediatR;
using VacationRental.Application.Common.ViewModel;

namespace VacationRental.Application.Rentals.Commands.PutRental
{
    public class PutRentalCommand : IRequest<ResourceIdViewModel>
    {
        public int Id { get; set; }
        public int Units { get; set; }
        public int PreparationTimeInDays { get; set; }
    }
}
