using MediatR;
using System.ComponentModel.DataAnnotations;

namespace VacationRental.Application.Bookings.Commands.CreateBooking
{
    public class CreateBookingCommand : IRequest<ResourceIdViewModel>
    {
        public int RentalId { get; set; }

        public DateTime Start { get; set; }

        [Range(1, int.MaxValue, ErrorMessage = "Nights must be positive")]
        public int Nights { get; set; }
    }
}
