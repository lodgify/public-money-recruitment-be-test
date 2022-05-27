using Flunt.Notifications;
using MediatR;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using VacationRental.Application.Notifications;
using VacationRental.Domain.Entities;
using VacationRental.Domain.ViewModels;
using VacationRental.Persistance.Interfaces;

namespace VacationRental.Application.Handlers.Rentals
{
    public class CreateRentalHandler : IRequestHandler<CreateRentalRequest,EntityResult<ResourceIdViewModel>>
    {
        private readonly IRepository<RentalEntity> _rentalRepository;

        public CreateRentalHandler(IRepository<RentalEntity> rentalRepository)
        {
            _rentalRepository = rentalRepository;
        }

        public async Task<EntityResult<ResourceIdViewModel>> Handle(CreateRentalRequest request, CancellationToken cancellationToken)
        {
            try
            {
                var entityResult = new EntityResult<ResourceIdViewModel>(request.Notifications, null);

                if (request.Valid)
                {
                    int id = _rentalRepository.GetAll().Select(p => p.Id).DefaultIfEmpty(0).Max() + 1;

                    var rental = new RentalEntity()
                    {
                        Id = id,
                        Units = request.Units,
                        PreparationTimeInDays = request.PreparationTimeInDays
                    };

                    await _rentalRepository.InsertAsync(rental);

                    var resourceIdViewModel = new ResourceIdViewModel() { Id = id };
                    entityResult = new EntityResult<ResourceIdViewModel>(request.Notifications, resourceIdViewModel);
                }

                return entityResult;
            }
            catch (Exception ex)
            {
                request.AddNotification(new Notification("CreateRentalHandler-Handler", $"Exception - [{ex.Message}]"));
                return new EntityResult<ResourceIdViewModel>(request.Notifications, null) { Error = ErrorCode.InternalServerError };
            }
        }
    }
}
