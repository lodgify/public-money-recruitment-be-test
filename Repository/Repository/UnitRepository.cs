using Models.DataModels;

namespace Repository.Repository;

public sealed class UnitRepository : IUnitRepository
{
    private readonly IDictionary<int, UnitDto> _dataSet;

    public UnitRepository(IDictionary<int, UnitDto> dataSet)
    {
        _dataSet = dataSet;
    }

    public async Task<bool> IsExists(int id)
    {
        return await Task.Run(() => _dataSet.ContainsKey(id));
    }

    public async Task<UnitDto> Get(int id)
    {
        return await Task.Run(() => _dataSet[id]);
    }

    public async Task<IEnumerable<UnitDto>> GetAll()
    {
        return await Task.Run(() => _dataSet.Values);
    }

    public async Task<IEnumerable<UnitDto>> GetAll(int rentalId)
    {
        return await Task.Run(() => _dataSet.Values.Where(_ => _.RentalId.Equals(rentalId)));
    }

    public async Task<UnitDto> Create(int id, UnitDto model)
    {
        await Task.Run(() => _dataSet.Add(id, model));

        return await Get(id);
    }

    public async Task<UnitDto> Update(int id, UnitDto model)
    {
        await Task.Run(() => _dataSet[id] = model);

        return await Get(id);
    }

    public async Task<bool> Delete(int unitId)
    {
        return await Task.Run(() => _dataSet.Remove(unitId));
    }
}
