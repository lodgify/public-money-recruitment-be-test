using System;
using System.Collections;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using VR.Application.Base;
using VR.Application.Resolvers;
using VR.DataAccess;
using VR.Domain.Models;
using VR.Infrastructure.Exceptions;
using VR.Infrastructure.Mapping.Interfaces;

namespace VR.Application.Requests.UpdateRental
{
    public class UpdateRentalRequestHandler : BaseRequestHandler<UpdateRentalRequest, UpdateRentalResponse>
    {
        private readonly IBookingConflictResolver _bookingConflictResolver;

        public UpdateRentalRequestHandler(IObjectMapper mapper, VRContext context, IBookingConflictResolver bookingConflictResolver) 
            : base(mapper, context) 
        {
            _bookingConflictResolver = bookingConflictResolver;
        }

        public async override Task<UpdateRentalResponse> Handle(UpdateRentalRequest request, CancellationToken cancellationToken)
        {
            var rental = await _context.Rentals.FindAsync(request.Id);
            
            if (rental == null)
                throw new NotFoundException("Rental is not  found", $"UpdateRentalRequest - rental with id {request.Id} not found");

            rental.Units = request.Units;
            rental.PreparationTimeInDays = request.PreparationTimeInDays;
            var rentalBookings = _context.Bookings.Where(x => x.RentalId == rental.Id);

            if (_bookingConflictResolver.HasBookingConflicts(rental, rentalBookings))
            {
                throw new BookingConflictsException("Booking Cross-Conflicts", $"UpdateRentalRequest - Could not update rental {request.Id} cause of Booking Conflicts");
            }

            _context.Rentals.Update(rental);
            await _context.SaveChangesAsync(cancellationToken);

            return _mapper.Map<Rental, UpdateRentalResponse>(rental);
        }
    }
}