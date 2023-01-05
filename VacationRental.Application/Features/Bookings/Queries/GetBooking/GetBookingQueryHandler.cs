using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using VacationRental.Application.Contracts.Mediatr;
using VacationRental.Application.Contracts.Persistence;
using VacationRental.Domain.Messages.Bookings;

namespace VacationRental.Application.Features.Bookings.Queries.GetBooking
{
    public class GetBookingQueryHandler : IQueryHandler<GetBookingQuery, BookingDto>
    {
        private readonly IBookingRepository _bookinRepository;
        private readonly IMapper _mapper;

        public GetBookingQueryHandler(IBookingRepository bookinRepository, IMapper mapper)
        {
            _bookinRepository = bookinRepository;
            _mapper = mapper;
        }

        public Task<BookingDto> Handle(GetBookingQuery request, CancellationToken cancellationToken)
        {
            var booking = _bookinRepository.GetById(request.bookingId);
            if (booking == null)
                throw new ApplicationException("Booking not found");

            return Task.FromResult(_mapper.Map<BookingDto>(booking));
        }
    }
}
