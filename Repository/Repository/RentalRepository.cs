using Models.DataModels;

namespace Repository.Repository;

public sealed class RentalRepository : IRentalRepository
{
    private readonly IDictionary<int, RentalDto> _dataSet;

    public RentalRepository(IDictionary<int, RentalDto> dataSet)
    {
        _dataSet = dataSet;
    }

    public async Task<bool> IsExists(int id)
    {
        return await Task.Run(() => _dataSet.ContainsKey(id));
    }

    public async Task<RentalDto> Get(int id)
    {
        return await Task.Run(() => _dataSet[id]);
    }

    public async Task<IEnumerable<RentalDto>> GetAll()
    {
        return await Task.Run(() => _dataSet.Values);
    }

    public async Task<RentalDto> Create(int id, RentalDto model)
    {
        await Task.Run(() => _dataSet.Add(id, model));

        return await Get(id);
    }

    public async Task<RentalDto> Update(int rentalId, RentalDto model)
    {
        await Task.Run(() => _dataSet[rentalId] = model);

        return await Get(rentalId);
    }
}
