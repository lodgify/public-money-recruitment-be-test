using VacationRental.Services.Dto;

namespace VacationRental.Services.Abstractions;

public interface IBookingService
{
    public BookingDto Get(int id);
    public int Create(BookingDto booking);
}
