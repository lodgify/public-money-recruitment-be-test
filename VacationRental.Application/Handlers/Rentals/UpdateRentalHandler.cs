using Flunt.Notifications;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using VacationRental.Application.Notifications;
using VacationRental.Domain.Entities;
using VacationRental.Domain.ViewModels;
using VacationRental.Persistance.Interfaces;

namespace VacationRental.Application.Handlers.Rentals
{
    public class UpdateRentalHandler : IRequestHandler<UpdateRentalRequest,EntityResult<RentalViewModel>>
    {
        private readonly IRepository<RentalEntity> _rentalRepository;
        private readonly IRepository<BookingEntity> _bookingRepository;

        public UpdateRentalHandler(IRepository<RentalEntity> repository, IRepository<BookingEntity> bookingRepository)
        {
            _rentalRepository = repository;
            _bookingRepository = bookingRepository;
        }

        public async Task<EntityResult<RentalViewModel>> Handle(UpdateRentalRequest request, CancellationToken cancellationToken)
        {
            try
            {
                var entityResult = new EntityResult<RentalViewModel>(request.Notifications, null);

                if (request.Valid)
                {
                    var rental = _rentalRepository.GetById(request.Id);

                    if (rental == null)
                    {
                        request.AddNotification(new Notification("UpdateRentalHandler-Handler", $"Exception - [Rental not found.]"));
                        return new EntityResult<RentalViewModel>(request.Notifications, null) { Error = ErrorCode.NotFound };
                    }

                    //cant remove units from update
                    if (rental.Units > request.Units)
                    {
                        request.AddNotification(new Notification("UpdateRentalHandler-Handler", $"Exception - [cant remove the unit]"));
                        return new EntityResult<RentalViewModel>(request.Notifications, null) { Error = ErrorCode.InternalServerError };
                    }

                    //verification for preparation time
                    if (request.PreparationTimeInDays > rental.PreparationTimeInDays)
                    {
                        var bookings = _bookingRepository.GetAll().Where(x => x.RentalId == request.Id).ToList();
                        var limitDate = rental.Units;

                        List<DateTime> bookingDates = new List<DateTime>();

                        foreach (var book in bookings)
                        {
                            var updatedEnd = book.Start.AddDays(book.Unit + request.PreparationTimeInDays);

                            var isEndDateScheduled = bookings.Exists(x => x.Start <= updatedEnd);

                            if (isEndDateScheduled)
                            {
                                request.AddNotification(new Notification("UpdateRentalHandler-Handler", $"Exception - [Cant be updated due to overlapping between existing bookings.]"));
                                return new EntityResult<RentalViewModel>(request.Notifications, null) { Error = ErrorCode.NotFound };
                            }
                        }
                    }

                    rental.PreparationTimeInDays = request.PreparationTimeInDays;
                    rental.Units = request.Units;

                    _rentalRepository.Update(rental);

                    var rentalViewModel = new RentalViewModel() { Id = rental.Id, Units = rental.Units, PreparationTimeInDays = rental.PreparationTimeInDays };
                    entityResult = new EntityResult<RentalViewModel>(request.Notifications, rentalViewModel);
                }

                return entityResult;
            }
            catch (Exception ex)
            {
                request.AddNotification(new Notification("UpdateRentalHandler-Handler", $"Exception - [{ex.Message}]"));
                return new EntityResult<RentalViewModel>(request.Notifications, null) { Error = ErrorCode.InternalServerError };
            }
        }
    }
}
