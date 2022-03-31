using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using VacationRental.Application.Common.ViewModel;

namespace VacationRental.Application.Rentals.Commands.PostRental
{
    public class PostRentalCommandHandler : IRequestHandler<PostRentalCommand, ResourceIdViewModel>
    {
        private readonly IDictionary<int, RentalViewModel> _rentals;

        public PostRentalCommandHandler(IDictionary<int, RentalViewModel> rentals)
        {
            _rentals = rentals;
        }

        public async Task<ResourceIdViewModel> Handle(PostRentalCommand request, CancellationToken cancellationToken)
        {
            var key = new ResourceIdViewModel { Id = _rentals.Keys.Count + 1 };

            _rentals.Add(key.Id, new RentalViewModel
            {
                Id = key.Id,
                Units = request.Units
            });

            return await Task.FromResult(key);
        }
    }
}