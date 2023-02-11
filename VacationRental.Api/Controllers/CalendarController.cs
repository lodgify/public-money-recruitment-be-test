using Microsoft.AspNetCore.Mvc;
using System;
using VacationRental.Application.Contracts.Pipeline;
using VacationRental.Application.Exceptions;
using VacationRental.Application.Features.Calendars.Queries.GetRentalCalendar;
using VacationRental.Domain.Messages.Calendars;

namespace VacationRental.Api.Controllers
{
    [ApiController]
    [Route("api/v1/[controller]")]
    public class CalendarController : ControllerBase
    {
        private readonly IQueryHandler<GetRentalCalendarQuery, CalendarDto> _getRentalCalendarQueryHandler;
        private readonly FluentValidation.IValidator<GetRentalCalendarQuery> _getRentalQueryValidator;

        public CalendarController(IQueryHandler<GetRentalCalendarQuery, CalendarDto> getRentalCalendarQueryHandler, FluentValidation.IValidator<GetRentalCalendarQuery> getRentalQueryValidator)
        {
            _getRentalCalendarQueryHandler = getRentalCalendarQueryHandler;
            _getRentalQueryValidator = getRentalQueryValidator;
        }

        [HttpGet]
        public CalendarDto Get(int rentalId, DateTime start, int nights)
        {
            var query = new GetRentalCalendarQuery(rentalId, start, nights);            
            var result = _getRentalQueryValidator.Validate(query);
            if (!result.IsValid)
            {
                throw new ValidationException(result.Errors);
            }
            return _getRentalCalendarQueryHandler.Handle(query);
        }
    }
}
