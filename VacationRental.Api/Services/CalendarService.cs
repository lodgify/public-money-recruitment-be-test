using LanguageExt;
using Microsoft.Extensions.Logging;
using System;
using VacationRental.Api.Core.Interfaces;
using VacationRental.Api.Core.Models;
using VacationRental.Api.Interfaces;
using VacationRental.Api.Models;

namespace VacationRental.Api.Services
{
    public class CalendarService : ICalendarService
    {
        private readonly ICalendarRepository _calendarRepository;
        private readonly ILogger<CalendarService> _logger;

        public CalendarService(ICalendarRepository calendarRepository, ILogger<CalendarService> logger)
        {
            _calendarRepository = calendarRepository;
            _logger = logger;
        }

        public Result<CalendarViewModel> GetCalendar(CalendarRequestModel request)
        {
            try
            {
                _logger.LogInformation($"Get data with value: {request}");
                return _calendarRepository.GetRentalCalendar(request.RentalId, request.Start, request.Nights);
            }
            catch (ApplicationException exception)
            {
                _logger.LogError($"An error occurred for method {nameof(GetCalendar)} with error:\n\n{exception.Message}");
                return new Result<CalendarViewModel>(exception);
            }
        }
    }

}
