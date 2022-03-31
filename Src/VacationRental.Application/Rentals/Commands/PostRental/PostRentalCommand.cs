using MediatR;
using VacationRental.Application.Common.ViewModel;

namespace VacationRental.Application.Rentals.Commands.PostRental
{
    public class PostRentalCommand : IRequest<ResourceIdViewModel>
    {
        public int Units { get; set; }
    }
}