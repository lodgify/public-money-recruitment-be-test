using Models.DataModels;

namespace Repository.Repository;

public interface IRentalRepository
{
    Task<bool> IsExists(int id);

    Task<RentalDto> Get(int id);

    Task<IEnumerable<RentalDto>> GetAll();

    Task<RentalDto> Create(int id, RentalDto model);

    Task<RentalDto> Update(int rentalId, RentalDto model);
}
