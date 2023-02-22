using Models.DataModels;

namespace Repository.Repository;

public sealed class BookingRepository : IBookingRepository
{
    private readonly IDictionary<int, BookingDto> _dataSet;

    public BookingRepository(IDictionary<int, BookingDto> dataSet)
    {
        _dataSet = dataSet;
    }

    public async Task<bool> IsExists(int id)
    {
        return await Task.Run(() => _dataSet.ContainsKey(id));
    }

    public async Task<BookingDto> Get(int id)
    {
        return await Task.Run(() => _dataSet[id]);
    }

    public async Task<IEnumerable<BookingDto>> GetAll()
    {
        return await Task.Run(() => _dataSet.Values);
    }

    public async Task<IEnumerable<BookingDto>> GetAll(int rentalId)
    {
        return await Task.Run(() => _dataSet.Values.Where(_ => _.RentalId.Equals(rentalId)));
    }

    public async Task<IEnumerable<BookingDto>> GetAll(int rentalId, DateTime orderStartDate)
    {
        return await Task.Run(() => _dataSet.Values.Where(_ =>
            _.RentalId.Equals(rentalId)
            && _.Start <= orderStartDate));
    }

    public async Task<IEnumerable<BookingDto>> GetAll(int rentalId, DateTime starDate, DateTime endDate)
    {
        return await Task.Run(() => _dataSet.Values.Where(_ =>
            _.RentalId.Equals(rentalId)
            && (CheckBookingCollisionTime(_, starDate) 
                || CheckBookingCollisionTime(_, endDate) 
                || CheckBookingInsideTimeInterval(_, starDate, endDate))
            ));
    }

    public async Task<BookingDto> Create(int id, BookingDto model)
    {
        await Task.Run(() => _dataSet.Add(id, model));

        return await Get(id);
    }

    public async Task<BookingDto> Update(int id, BookingDto model)
    {
        await Task.Run(() => _dataSet[id] = model);

        return await Get(id);
    }

    /// <summary>
    /// returns true if passed Date value inside of entity booking date interval
    /// </summary>
    /// <param name="entity"></param>
    /// <param name="date"></param>
    /// <returns></returns>
    private bool CheckBookingCollisionTime(BookingDto entity, DateTime date) =>
        entity.Start <= date
        && entity.Start.AddDays(entity.Nights + entity.PreparationTimeInDays) >= date;

    /// <summary>
    /// returns true if entity date interval inside of passed start and end values
    /// </summary>
    /// <param name="entity"></param>
    /// <param name="start"></param>
    /// <param name="end"></param>
    /// <returns></returns>
    private bool CheckBookingInsideTimeInterval(BookingDto entity, DateTime start, DateTime end) =>
        entity.Start >= start
        && entity.Start.AddDays(entity.Nights + entity.PreparationTimeInDays) <= end;
}
