using Models.ViewModels;

namespace VacationRental.Api.Repository;

public sealed class BookingRepository : IBookingRepository
{
    private readonly IDictionary<int, BookingViewModel> _dataSet;

    public BookingRepository(IDictionary<int, BookingViewModel> dataSet)
    {
        _dataSet = dataSet;
    }

    public async Task<bool> IsExists(int id)
    {
        return await Task.Run(() => _dataSet.ContainsKey(id));
    }

    public async Task<BookingViewModel> Get(int id)
    {
        return await Task.Run(() => _dataSet[id]);
    }

    public async Task<IEnumerable<BookingViewModel>> GetAll()
    {
        return await Task.Run(() => _dataSet.Values);
    }

    public async Task<BookingViewModel> Create(int id, BookingViewModel model)
    {
        await Task.Run(() => _dataSet.Add(id, model));

        return await Get(id);
    }
}
