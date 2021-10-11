using System;
using System.Collections.Generic;
using VacationRental.Domain.Interfaces;

namespace VacationRental.Infrastructure.DataBase
{
    public class EFContext
    {
        private readonly Dictionary<Type, Dictionary<Guid, IBaseEntity>> _context;

        public EFContext(Dictionary<Type, Dictionary<Guid, IBaseEntity>> initialStatus)
        {
            _context = initialStatus;
        }

        public void getStore<Entity>(ref Dictionary<Guid, IBaseEntity> entityStore) where Entity : IBaseEntity
        {
            if (!_context.ContainsKey(typeof(Entity)))
            {
                _context.Add(typeof(Entity), new Dictionary<Guid, IBaseEntity>());
            }
            entityStore = _context[typeof(Entity)];
        }

        public Dictionary<Type, Dictionary<Guid, IBaseEntity>> ToDatabase()
        {
            return this._context;
        }
    }
}
