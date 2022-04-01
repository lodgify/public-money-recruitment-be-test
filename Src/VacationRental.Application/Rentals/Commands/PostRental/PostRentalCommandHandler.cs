using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using VacationRental.Application.Common.ViewModel;
using VacationRental.Domain.Rentals;

namespace VacationRental.Application.Rentals.Commands.PostRental
{
    public class PostRentalCommandHandler : IRequestHandler<PostRentalCommand, ResourceIdViewModel>
    {
        private readonly IRentalRepository _repository;

        public PostRentalCommandHandler(IRentalRepository repository)
        {
            _repository = repository;
        }

        public async Task<ResourceIdViewModel> Handle(PostRentalCommand request, CancellationToken cancellationToken)
        {
            var key = _repository.Add(new RentalModel()
            {
                Units = request.Units,
                PreparationTimeInDays = request.PreparationTimeInDays   
            }); 
            
            return await Task.FromResult(new ResourceIdViewModel() {Id = key});
        }
    }
}
