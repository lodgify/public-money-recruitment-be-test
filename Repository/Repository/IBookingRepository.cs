using Models.DataModels;

namespace Repository.Repository;

public interface IBookingRepository
{
    Task<bool> IsExists(int id);

    Task<BookingDto> Get(int id);

    Task<IEnumerable<BookingDto>> GetAll();

    Task<IEnumerable<BookingDto>> GetAll(int rentalId);

    Task<IEnumerable<BookingDto>> GetAll(int rentalId, DateTime orderStartDate);

    Task<IEnumerable<BookingDto>> GetAll(int rentalId, DateTime starDate, DateTime endDate);

    Task<BookingDto> Create(int id, BookingDto model);

    Task<BookingDto> Update(int id, BookingDto model);
}
