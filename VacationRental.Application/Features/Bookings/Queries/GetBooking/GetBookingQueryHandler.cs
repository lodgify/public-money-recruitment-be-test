using AutoMapper;
using System;
using VacationRental.Application.Contracts.Pipeline;
using VacationRental.Application.Contracts.Persistence;
using VacationRental.Domain.Messages.Bookings;
using VacationRental.Application.Exceptions;
using VacationRental.Domain.Errors;

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

        public BookingDto Handle(GetBookingQuery request)
        {            

            var booking = _bookinRepository.GetById(request.bookingId);
            if (booking == null)
                throw new NotFoundException(BookingError.BookingNotFound);

            return _mapper.Map<BookingDto>(booking);
        }        
    }
}
