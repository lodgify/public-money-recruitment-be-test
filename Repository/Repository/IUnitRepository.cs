using Models.DataModels;

namespace Repository.Repository;

public interface IUnitRepository
{
    Task<bool> IsExists(int id);

    Task<UnitDto> Get(int id);

    Task<IEnumerable<UnitDto>> GetAll();

    Task<IEnumerable<UnitDto>> GetAll(int rentalId);

    Task<UnitDto> Create(int id, UnitDto model);

    Task<UnitDto> Update(int unitId, UnitDto model);

    Task<bool> Delete(int unitId);
}
