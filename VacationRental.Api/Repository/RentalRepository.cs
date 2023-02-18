using Models.ViewModels;

namespace VacationRental.Api.Repository;

public sealed class RentalRepository : IRentalRepository
{
    private readonly IDictionary<int, RentalViewModel> _dataSet;

    public RentalRepository(IDictionary<int, RentalViewModel> dataSet)
    {
        _dataSet = dataSet;
    }

    public bool IsExists(int id)
    {
        return _dataSet.ContainsKey(id);
    }

    public RentalViewModel Get(int id)
    {
        return _dataSet[id];
    }

    public IEnumerable<RentalViewModel> GetAll()
    {
        return _dataSet.Values;
    }

    public RentalViewModel Create(int id, RentalViewModel model)
    {
        _dataSet.Add(id, model);

        return Get(id);
    }
}
