using System.Collections.Generic;
using VacationRental.Repository.Interfaces;

namespace VacationRental.Service.Interfaces
{
    public interface IBaseService<TRepository, TDTO, TEntity>
		where TRepository : IBaseRepository<TEntity>
		where TDTO : class
		where TEntity : class
	{
		TDTO Get(int id);

		List<TDTO> GetAll();

		TDTO Add(TDTO item);

		TDTO AddOrUpdate(TDTO item);
		bool Remove(int id);
	}
}
