using Application.Models.Calendar.Requests;
using Application.Models.Calendar.Responses;
using MassTransit;
using Persistence.Contracts.Interfaces;

namespace Application.Features.Calendar.Queries;

public class GetCalendarQueryConsumer : IConsumer<CalendarRequest>
{
    private readonly IRepository<Domain.Entities.Booking> _repository;

    public GetCalendarQueryConsumer(IRepository<Domain.Entities.Booking> repository)
    {
        _repository = repository;
    }

    public async Task Consume(ConsumeContext<CalendarRequest> context)
    {
        var request = context.Message;

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

            var bookings = await _repository.GetAllAsync(x => x.RentalId == request.RentalId &&
                                                              (x.Start > date.Date || x.LastDay <= date.Date)
            );

            bookings.ForEach(x =>
            {
                if (x.IsPreparation)
                {
                    date.PreparationTimes.Add(new PreparationTime() { Unit = x.Unit });
                }
                else
                {
                    date.Bookings.Add(new CalendarBookingViewModel { Id = x.Id, Unit = x.Unit });
                }
            });

            result.Dates.Add(date);
        }

        await context.RespondAsync(result);
    }
}