using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using VacationRental.Application.Common.Services;
using VacationRental.Domain.Bookings;
using VacationRental.Domain.Rentals;

namespace VacationRental.Application.Calendars.Queries.GetCalendar
{
    public class GetCalendarQueryHandler : IRequestHandler<GetCalendarQuery, CalendarViewModel>
    {
        private readonly IRentalRepository _rentalRepository;
        private readonly IBookingRepository _bookingRepository;
        private readonly IBookingSearchService _bookingSearchService;
        public GetCalendarQueryHandler(IRentalRepository rentalRepository, IBookingRepository bookingRepository, IBookingSearchService bookingSearchService)
        {
            _rentalRepository = rentalRepository;
            _bookingRepository = bookingRepository;
            _bookingSearchService = bookingSearchService;
        }

        public async Task<CalendarViewModel> Handle(GetCalendarQuery request, CancellationToken cancellationToken)
        {
            var rental = _rentalRepository.Get(request.RentalId);

            if (rental == null)
                return null;

            var result = new CalendarViewModel
            {
                RentalId = request.RentalId,
                Dates = new List<CalendarDateViewModel>()
            };
            
            var bookingsByRental = _bookingRepository.GetByRentalId(rental.Id)?.ToList();

            if (bookingsByRental != null && bookingsByRental.Count == 0) return null;

            for (var i = 0; i < request.Nights; i++)
            {
                var currentDate = request.Start.Date.AddDays(i);
                
                var date = new CalendarDateViewModel
                {
                    Date = currentDate,
                    Bookings = new List<CalendarBookingViewModel>(),
                    PreparationTime = new List<PreparationTimeViewModel>()
                };

                var bookings = _bookingSearchService.GetBookingsByDay(new GetBookingsByDayDTO()
                {
                    Bookings = bookingsByRental,
                    Day = currentDate
                });

                var preparationTime = _bookingSearchService.GetBookingsByDay(new GetBookingsByDayDTO()
                {
                    Bookings = bookingsByRental,
                    Day = currentDate,
                    PreparationTime = rental.PreparationTimeInDays
                }).Where(x => !bookings.Contains(x));

                var bookingsTasks = AddBookingsToCalendarDateViewModel(bookings, date, cancellationToken);
                var preparationTimeTask = AddPreparationTimeToCalendarDateViewModel(date, preparationTime, cancellationToken);

                Task.WaitAll(bookingsTasks, preparationTimeTask);
                result.Dates.Add(date);
            }

            return await Task.FromResult(result);
        }

        private static Task AddPreparationTimeToCalendarDateViewModel(CalendarDateViewModel date, IEnumerable<BookingModel> preparationTimes,
            CancellationToken cancellationToken)
        {
            return Task.Factory.StartNew(() =>
            {
                foreach (var pT in preparationTimes)
                {
                    date.PreparationTime.Add(new PreparationTimeViewModel()
                    {
                        Unit = date.PreparationTime.Count + 1
                    });
                }
            }, cancellationToken);
        }

        private static Task AddBookingsToCalendarDateViewModel(IEnumerable<BookingModel> bookings, CalendarDateViewModel date,
            CancellationToken cancellationToken)
        {
            return Task.Factory.StartNew(() =>
            {
                foreach (var bookingModel in bookings)
                {
                    date.Bookings.Add(new CalendarBookingViewModel()
                    {
                        Id = bookingModel.Id,
                        Unit = date.Bookings.Count + 1
                    });
                }
            }, cancellationToken);
        }
    }
}