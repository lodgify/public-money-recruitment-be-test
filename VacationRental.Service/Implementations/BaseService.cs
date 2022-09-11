using AutoMapper;
using System.Collections.Generic;
using System.Linq;
using VacationRental.Repository.Interfaces;
using VacationRental.Service.Interfaces;

namespace VacationRental.Service.Implementations
{
    public class BaseService<TRepository, TDTO, TEntity> : IBaseService<TRepository, TDTO, TEntity>
		where TRepository : IBaseRepository<TEntity>
		where TDTO : class
		where TEntity : class
	{
		protected readonly TRepository _repository;
		protected readonly IMapper _mapper;

		public BaseService(IMapper mapper, TRepository repository)
		{
			_repository = repository;
			_mapper = mapper;
		}

		public TDTO AddOrUpdate(TDTO item)
		{
			int id = 0;
			int.TryParse(item.GetType().GetProperty("Id").GetValue(item).ToString(), out id);
			if (id > 0)
			{
				return Update(item, id);
			}
			else
			{
				return Add(item);
			}
		}

		public TDTO Update(TDTO item, object id)
		{
			var entity = _mapper.Map<TEntity>(item);
			entity = _repository.Update(entity, id);
			item = _mapper.Map<TDTO>(entity);
			return item;
		}

		public TDTO Add(TDTO item)
		{
			var entity = _mapper.Map<TEntity>(item);
			entity = _repository.Add(entity);
			item = _mapper.Map<TDTO>(entity);
			return item;
		}

		public TDTO Get(int id)
		{
			var item = _repository.Get(id);
			return _mapper.Map<TDTO>(item);
		}

		public List<TDTO> GetAll()
		{
			var items = _repository.GetAll().ToList().Select(p => _mapper.Map<TDTO>(p)).ToList();
			return items;
		}

		public bool Remove(int id)
		{
			_repository.Delete(id);
			return true;
		}
	}
}
