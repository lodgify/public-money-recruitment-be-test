using System.Collections.Generic;
using VacationRental.Api.Services.Interfaces;

namespace VacationRental.Api.Services
{
    public class GeneralService<T> : IGeneralService<T> where T : class
    {
        public IDictionary<int, T> Dictionary;

        public GeneralService(IDictionary<int, T> dictionary)
        {
            Dictionary = dictionary;
        }

        public IDictionary<int, T> Get()
            => Dictionary;

        public T Get(int id)
            => Dictionary[id] ?? null;

        public void Add(int key, T entity)
            => Dictionary.Add(key, entity);

        public void Update(int key, T entity)
        {
            Dictionary[key] = entity;
        }

        public int GenerateId()
            => Dictionary.Keys.Count + 1;
    }
}
