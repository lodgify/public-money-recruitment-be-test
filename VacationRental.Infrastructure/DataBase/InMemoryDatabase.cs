using System;
using System.Collections.Generic;
using System.Linq;
using VacationRental.Domain.Interfaces;

namespace VacationRental.Infrastructure.DataBase
{
    class InMemoryDatabase
    {
        private static readonly Dictionary<Type, Dictionary<Guid, IBaseEntity>> _database = new Dictionary<Type, Dictionary<Guid, IBaseEntity>>();

        internal Dictionary<Type, Dictionary<Guid, IBaseEntity>> copyDatabase()
        {
            return _database.ToDictionary(entry => entry.Key, entry => entry.Value);
        }

        public EFContext createContext()
        {
            return new EFContext(copyDatabase());
        }

        /*
        public static void Merge<TKey, TValue>(this IDictionary<TKey, TValue> first
            , IDictionary<TKey, TValue> second
            , Func<TValue, TValue, TValue> aggregator)
        {
            if (second == null) return;
            if (first == null) throw new ArgumentNullException("first");
            foreach (var item in second)
            {
                if (!first.ContainsKey(item.Key))
                {
                    first.Add(item.Key, item.Value);
                }
                else
                {
                   first[item.Key] = aggregator(first[item.key], item.Value);
                }
            }
        }

        */
        public void commit(Dictionary<Type, Dictionary<Guid, IBaseEntity>> newStatus)
        {
            lock (newStatus)
            {
                foreach (KeyValuePair<Type, Dictionary<Guid, IBaseEntity>> repoUpdate in newStatus)
                {
                    if (!_database.ContainsKey(repoUpdate.Key))
                    {
                        _database[repoUpdate.Key] = new Dictionary<Guid, IBaseEntity>();
                    }

                    lock (_database[repoUpdate.Key])
                    {
                        foreach (Guid EntityGuid in repoUpdate.Value.Keys)
                        {
                            _database[repoUpdate.Key][EntityGuid] = repoUpdate.Value[EntityGuid];

                        }
                    }
                }
            }
        }
    }
}
