using Flunt.Notifications;
using MediatR;
using System;
using System.Threading;
using System.Threading.Tasks;
using VacationRental.Application.Notifications;
using VacationRental.Domain.Entities;
using VacationRental.Domain.ViewModels;
using VacationRental.Persistance.Interfaces;

namespace VacationRental.Application.Handlers.Rentals
{
    public class GetRentalHandler : IRequestHandler<GetRentalRequest, EntityResult<RentalViewModel>>
    {
        private readonly IRepository<RentalEntity> _rentalRepository;

        public GetRentalHandler(IRepository<RentalEntity> repository)
        {
            _rentalRepository = repository;
        }

        async Task<EntityResult<RentalViewModel>> IRequestHandler<GetRentalRequest, EntityResult<RentalViewModel>>.Handle(GetRentalRequest request, CancellationToken cancellationToken)
        {
            try
            {
                var entityResult = new EntityResult<RentalViewModel>(request.Notifications, null);

                if (entityResult.Valid)
                {
                    var result = _rentalRepository.GetById(request.Id);

                    if (result == null)
                    {
                        return new EntityResult<RentalViewModel>(request.Notifications, null);
                    }
                    if (result != null)
                    {
                        var rentalviewModel = new RentalViewModel()
                        {
                            Id = result.Id,
                            Units = result.Units,
                            PreparationTimeInDays = result.PreparationTimeInDays
                        };
                        entityResult = new EntityResult<RentalViewModel>(request.Notifications, rentalviewModel);
                    }
                }

                return entityResult;
            }
            catch (Exception ex)
            {
                request.AddNotification(new Notification("GetRentalHandler-Handler", $"Exception - [{ex.Message}]"));
                return new EntityResult<RentalViewModel>(request.Notifications, null) { Error = ErrorCode.InternalServerError };
            }
        }
    }
}
