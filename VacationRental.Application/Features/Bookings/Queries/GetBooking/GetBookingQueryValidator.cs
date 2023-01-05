using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VacationRental.Application.Features.Bookings.Queries.GetBooking
{
    public class GetBookingQueryValidator : AbstractValidator<GetBookingQuery>
    {
        public GetBookingQueryValidator() 
        {
            RuleFor(c => c.bookingId)
                .NotNull().WithMessage("BookingId can not be null");

        }
    }
}
