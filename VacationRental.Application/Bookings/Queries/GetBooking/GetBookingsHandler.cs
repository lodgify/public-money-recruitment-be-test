using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using VacationRental.Application.Bookings.Models;
using VacationRental.Domain;

namespace VacationRental.Application.Bookings.Queries.GetBooking
{
    public class GetBookingsHandler : IRequestHandler<GetBookingsQuery, BookingViewModel>
    {
        private readonly VacationRentalDbContext _dbContext;
        private readonly IMapper _mapper;

        public GetBookingsHandler(VacationRentalDbContext dbContext, IMapper mapper)
        {
            _dbContext = dbContext;
            _mapper = mapper;
        }

        public async Task<BookingViewModel> Handle(GetBookingsQuery request, CancellationToken cancellationToken)
        {
            var booking = await _dbContext.Bookings
                .AsNoTracking()
                .Include(b => b.Unit)
                .FirstOrDefaultAsync(b => b.Id == request.BookingId, cancellationToken);

            if (booking == null)
            {
                throw new ValidationException("Booking not found");
            }

            return _mapper.Map<BookingViewModel>(booking);
        }
    }
}
