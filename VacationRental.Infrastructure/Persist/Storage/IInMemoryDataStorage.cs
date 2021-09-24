using System;
using System.Collections.Generic;

namespace VacationRental.Infrastructure.Persist.Storage
{
    public interface IInMemoryDataStorage<TModel>
    where TModel : class, new()
    {
        int Add(TModel model);
        bool TryGetValue(int key, out TModel model);
        IReadOnlyCollection<TModel> Get(Func<TModel, bool> specification);
    }

}
