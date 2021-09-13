using System;
using System.Collections.Generic;
using System.Linq;

namespace VacationRental.Data
{
	public interface IRepository<TEntity> : IDisposable
	where TEntity : class
	{
		void Add(TEntity entity);

		void Delete(TEntity entity);

		IQueryable<TEntity> GetAll();

		TEntity GetById(long id);

		void SaveChanges();

		void Update(TEntity entity);

        IEnumerable<TEntity> Where(Func<TEntity, bool> parameters);
    }
}