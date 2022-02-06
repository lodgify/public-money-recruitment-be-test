using System.Threading;
using System.Threading.Tasks;
using Application.Business.Commands.Booking;
using Application.Models.Rental.Requests;
using Domain.DAL;
using MediatR;

namespace Application.Business.Commands.Rental
{
    public class UpdateRentalCommand : IRequest<Unit>
    {
        public UpdateRentalRequest Request { get; }
        
        public UpdateRentalCommand(UpdateRentalRequest request)
        {
            Request = request;
        }
    }
    
    public class UpdateRentalCommandHandler  : IRequestHandler<UpdateRentalCommand, Unit>
    {
        private readonly IRepository<Domain.DAL.Models.Rental> _repository;
        private readonly IMediator _mediator;

        public UpdateRentalCommandHandler(IRepository<Domain.DAL.Models.Rental> repository, IMediator mediator)
        {
            _repository = repository;
            _mediator = mediator;
        }

        public async Task<Unit> Handle(UpdateRentalCommand command, CancellationToken cancellationToken)
        {
            var request = command.Request;

            var oldRental = _repository.Find(request.Id);
            
          _repository.Update(new Domain.DAL.Models.Rental()
          {
              Id = request.Id,
              Units = request.Units,
              PreparationTimeInDays = request.PreparationTimeInDays
          });

          if (oldRental.PreparationTimeInDays != request.PreparationTimeInDays)
          {
              await _mediator.Send(new RecalculatePreparationCommand(request.Id), cancellationToken);
          }

          return Unit.Value;
        }
    }
}