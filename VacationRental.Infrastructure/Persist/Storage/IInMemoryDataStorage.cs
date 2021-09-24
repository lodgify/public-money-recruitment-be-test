using System;
using System.Collections.Generic;

namespace VacationRental.Infrastructure.Persist.Storage
{
    public interface IInMemoryDataStorage<TModel>
    where TModel : class, new()
    {
        TModel Add(TModel model);
        bool TryGetAsync(int Key, out TModel model);
        IReadOnlyCollection<TModel> Get(Func<TModel, bool> specification);
    }

}
