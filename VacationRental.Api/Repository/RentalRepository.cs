using Models.ViewModels;

namespace VacationRental.Api.Repository;

public sealed class RentalRepository : IRentalRepository
{
    private readonly IDictionary<int, RentalViewModel> _dataSet;

    public RentalRepository(IDictionary<int, RentalViewModel> dataSet)
    {
        _dataSet = dataSet;
    }

    public async Task<bool> IsExists(int id)
    {
        return await Task.Run(() => _dataSet.ContainsKey(id));
    }

    public async Task<RentalViewModel> Get(int id)
    {
        return await Task.Run(() => _dataSet[id]);
    }

    public async Task<IEnumerable<RentalViewModel>> GetAll()
    {
        return await Task.Run(() => _dataSet.Values);
    }

    public async Task<RentalViewModel> Create(int id, RentalViewModel model)
    {
        await Task.Run(() => _dataSet.Add(id, model));

        return await Get(id);
    }
}
