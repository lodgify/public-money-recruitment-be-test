using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Application.Models.Calendar.Requests;
using Application.Models.Calendar.Responses;
using Domain.DAL;
using MediatR;
using VacationRental.Api.Models;

namespace Application.Business.Queries.Calendar
{
    public class GetCalendarQuery : IRequest<CalendarViewModel> 
    {
        public GetCalendarRequest Request { get; }
        
        public GetCalendarQuery(GetCalendarRequest request)
        {
            Request = request;
        }
    }
    
    public class GetCalendarQueryHandler : IRequestHandler<GetCalendarQuery,CalendarViewModel>
    {
        private readonly IRepository<Domain.DAL.Models.Booking> _repository;

        public GetCalendarQueryHandler(IRepository<Domain.DAL.Models.Booking> repository)
        {
            _repository = repository;
        }

        public Task<CalendarViewModel> Handle(GetCalendarQuery query, CancellationToken cancellationToken)
        {
            var request = query.Request;
            
            var result = new CalendarViewModel 
            {
                RentalId = request.RentalId,
                Dates = new List<CalendarResponse>() 
            };
            for (var i = 0; i < request.Nights; i++)
            {
                var date = new CalendarResponse
                {
                    Date = request.Start.Date.AddDays(i),
                    Bookings = new List<CalendarBookingViewModel>(),
                    PreparationTimes = new List<PreparationTime>()
                };

                foreach (var booking in _repository.Query.Values)
                {
                    if (booking.RentalId != request.RentalId) continue;
                    
                    if (booking.Start > date.Date || booking.LastDay <= date.Date) continue;
                    
                    if (booking.IsPreparation)
                    {
                        date.PreparationTimes.Add(new PreparationTime() { Unit = booking.Unit});
                    }
                    else
                    {
                        date.Bookings.Add(new CalendarBookingViewModel { Id = booking.Id, Unit = booking.Unit});
                    }

                }

                result.Dates.Add(date);
            }

            return Task.FromResult(result);
        }
    }
}