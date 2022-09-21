using VacationRental.Services.Dto;

namespace VacationRental.Services.Abstractions;

public interface ICalendarService
{
    public CalendarDto Get(CalendarFilterDto filter);
}
