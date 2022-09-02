using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using VR.Application.Base;
using VR.Application.Resolvers;
using VR.DataAccess;
using VR.Domain.Models;
using VR.Infrastructure.Exceptions;
using VR.Infrastructure.Mapping.Interfaces;

namespace VR.Application.Requests.AddBooking
{
    public class AddBookingRequestHandler : BaseRequestHandler<AddBookingRequest, AddBookingResponse>
    {
        private readonly IBookingConflictResolver _bookingConflictResolver;

        public AddBookingRequestHandler(IObjectMapper mapper, VRContext context, IBookingConflictResolver bookingConflictResolver) 
            : base(mapper, context) 
        {
            _bookingConflictResolver = bookingConflictResolver;
        }

        public async override Task<AddBookingResponse> Handle(AddBookingRequest request, CancellationToken cancellationToken)
        {
            var booking = new Booking
            {
                Nights = request.Nights,
                RentalId = request.RentalId,
                Start = request.Start.Date
            };

            var rental = await _context.Rentals.FindAsync(request.RentalId);
            if (rental == null)
                throw new NotFoundException("Rental is not  found", $"AddBookingRequest - rental with id {request.RentalId} not found");

            var existingRentalBookings = _context.Bookings.Where(x => x.RentalId == booking.RentalId).ToList();
            var units = _context.Rentals.Find(booking.RentalId).Units;

            var bookedUnits = _bookingConflictResolver.GetCrossBookedUnits(rental, booking, existingRentalBookings);

            if (bookedUnits.Count() >= units)
            {
                throw new BookingConflictsException("Booking Cross-Conflicts", $"AddBookingRequest - Could not add booking request cause of Booking Cross-Conflicts");
            } 

            booking.Unit = _bookingConflictResolver.GetAvailableUnit(rental, bookedUnits);

            await _context.Bookings.AddAsync(booking, cancellationToken);
            await _context.SaveChangesAsync(cancellationToken);

            return _mapper.Map<Booking, AddBookingResponse>(booking);
        }
    }
}