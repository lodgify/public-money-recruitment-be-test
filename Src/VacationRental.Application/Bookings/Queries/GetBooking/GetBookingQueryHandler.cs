using System;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using VacationRental.Domain.Bookings;

namespace VacationRental.Application.Bookings.Queries.GetBooking
{
    public class GetBookingQueryHandler : IRequestHandler<GetBookingQuery, BookingViewModel>
    {
        private readonly IBookingRepository _repository;

        public GetBookingQueryHandler(IBookingRepository repository)
        {
            _repository = repository;
        }

        public async Task<BookingViewModel> Handle(GetBookingQuery request, CancellationToken cancellationToken)
        {
            var model = _repository.Get(request.BookingId);

            if (model == null)
                return null;
    
            return await Task.FromResult(new BookingViewModel()
            {
                Id = model.Id,
                Nights = model.Nights,
                Start = model.Start,
                RentalId = model.RentalId
            });
        }
    }
}