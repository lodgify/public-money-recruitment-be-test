using Flunt.Notifications;
using Flunt.Validations;
using MediatR;
using System;
using VacationRental.Application.Notifications;
using VacationRental.Domain.ViewModels;

namespace VacationRental.Application.Handlers.Bookings
{
    public class CreateBookingRequest: Notifiable, IRequest<EntityResult<ResourceIdViewModel>>
    {
        public int Nights { get; set; }
        public int RentalId { get; set; }
        public DateTime StartDate { get; set; }

        public CreateBookingRequest(DateTime startDate, int nights, int rentalId)
        {
            Nights = nights;
            RentalId = rentalId;
            StartDate = startDate;

            AddNotifications(new Contract()
                .IsGreaterThan(Nights, 0, "Nights", "Nights must be grater than 0.")
                .IsGreaterOrEqualsThan(StartDate, DateTime.Now.Date, "StartDate", "Start date must be equal or greater than current Date."));
        }
    }
}
