using System.Threading;
using System.Threading.Tasks;
using Application.Models;
using Domain.DAL;
using MediatR;
using VacationRental.Api.Models;

namespace Application.Business.Commands.Rental
{
    public class CreateRentalCommand : IRequest<ResourceIdViewModel>
    {
        public CreateRentalRequest Request { get;}
        
        public CreateRentalCommand(CreateRentalRequest request)
        {
            Request = request;
        }
    }
    
    public class CreateRentalCommandHandler : IRequestHandler<CreateRentalCommand, ResourceIdViewModel>
    {
        private readonly IRepository<Domain.DAL.Models.Rental> _repository;

        public CreateRentalCommandHandler(IRepository<Domain.DAL.Models.Rental> repository)
        {
            _repository = repository;
        }

        public Task<ResourceIdViewModel> Handle(CreateRentalCommand command, CancellationToken cancellationToken)
        {
            var model = command.Request;

            var id = _repository.Insert(
                new Domain.DAL.Models.Rental
                {
                    Units = model.Units,
                    PreparationTimeInDays = model.PreparationTimeInDays
                });

            return Task.FromResult(new ResourceIdViewModel {Id = id});
        }
    }
}