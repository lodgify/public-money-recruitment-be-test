using System.Threading;
using System.Threading.Tasks;
using Application.Models.Booking.Responses;
using AutoMapper;
using Domain.DAL;
using MediatR;

namespace Application.Business.Queries.Booking
{
    public class GetBookingQuery : IRequest<BookingResponse>
    {
        public int BookingId { get; }
        
        public GetBookingQuery(int bookingId)
        {
            BookingId = bookingId;
        }
    }
    
    public class GetBookingQueryHandler : IRequestHandler<GetBookingQuery, BookingResponse>
    {
        private readonly IMapper _mapper;
        private readonly IRepository<Domain.DAL.Models.Booking> _repository;

        public GetBookingQueryHandler(IMapper mapper, IRepository<Domain.DAL.Models.Booking> repository)
        {
            _mapper = mapper;
            _repository = repository;
        }

        public Task<BookingResponse> Handle(GetBookingQuery request, CancellationToken cancellationToken)
        {
            return Task.FromResult(_mapper.Map<BookingResponse>(_repository.Find(request.BookingId)));
        }
    }
}