using System;
using MediatR;

namespace VacationRental.Application.Commands.Booking
{
    public class BookingCommandRequest : IRequest<ResourceIdViewModel>
    {
        public int RentalId { get; set; }

        public DateTime Start
        {
            get => _startIgnoreTime;
            set => _startIgnoreTime = value.Date;
        }

        private DateTime _startIgnoreTime;
        public int Nights { get; set; }
    }
}


