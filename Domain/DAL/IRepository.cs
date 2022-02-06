using System.Collections.Generic;
using Domain.DAL.Models;

namespace Domain.DAL
{
    public interface IRepository<TEntity> where TEntity : BaseEntity
    {
        int Insert(TEntity entity);

        TEntity Find(int key);

        void Update(TEntity entity);

        IDictionary<int,TEntity> Query { get; }

    }
}