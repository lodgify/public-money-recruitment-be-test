using Models.ViewModels;

namespace VacationRental.Api.Repository;

public sealed class BookingRepository : IBookingRepository
{
    private readonly IDictionary<int, BookingViewModel> _dataSet;

    public BookingRepository(IDictionary<int, BookingViewModel> dataSet)
    {
        _dataSet = dataSet;
    }

    public bool IsExists(int id)
    {
        return _dataSet.ContainsKey(id);
    }

    public BookingViewModel Get(int id)
    {
        return _dataSet[id];
    }

    public IEnumerable<BookingViewModel> GetAll()
    {
        return _dataSet.Values;
    }

    public BookingViewModel Create(int id, BookingViewModel model)
    {
        _dataSet.Add(id, model);

        return Get(id);
    }
}
