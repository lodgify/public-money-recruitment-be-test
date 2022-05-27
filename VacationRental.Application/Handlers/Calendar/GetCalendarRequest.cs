using Flunt.Notifications;
using Flunt.Validations;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;
using VacationRental.Application.Notifications;
using VacationRental.Domain.ViewModels;

namespace VacationRental.Application.Handlers.Calendar
{
    public class GetCalendarRequest : Notifiable, IRequest<EntityResult<CalendarViewModel>>
    {
        public int RentalId { get; set; }
        public int Nights { get; set; }
        public DateTime Start { get; set; }
        public GetCalendarRequest(int rentalId, DateTime start, int nights)
        {
            RentalId = rentalId;
            Nights = nights;
            Start = start;

            AddNotifications(new Contract()
                .IsGreaterThan(rentalId, 0, "Id", "Id must be greater than 0."));
        }
    }
}
