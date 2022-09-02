using MediatR;
using System;

namespace VR.Application.Requests.AddBooking
{
    public class AddBookingRequest : IRequest<AddBookingResponse>
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
