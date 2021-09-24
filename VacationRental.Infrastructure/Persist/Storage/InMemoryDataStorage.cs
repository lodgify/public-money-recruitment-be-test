using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;

namespace VacationRental.Infrastructure.Persist.Storage
{

    public sealed class InMemoryDataStorage<TModel> : IInMemoryDataStorage<TModel>
        where TModel : class, new()
    {

        private int _lastKeyValue = 0;
        private readonly Dictionary<int, TModel> _items = new Dictionary<int, TModel>();
        private readonly Action<TModel, int> _setNextKey;

        public InMemoryDataStorage(Expression<Func<TModel, int>> keyPropertyExpression)
        {
            _setNextKey = CompileKeySetter(keyPropertyExpression);
        }

        public int Add(TModel model)
        {
            var newKey = _lastKeyValue++;
            _setNextKey(model, newKey);
            _items.Add(newKey, model);

            return newKey;
        }

        public bool TryGetValue(int key, out TModel model) =>
            _items.TryGetValue(key, out model);

        public IReadOnlyCollection<TModel> Get(Func<TModel, bool> specification) =>
            _items.Values.Where(specification).ToList();


        /// <summary>
        /// Generates a lambda setting a key value for an item.
        /// </summary>
        /// <param name="keyPropertyExpression"></param>
        /// <returns></returns>
        private Action<TModel, int> CompileKeySetter(Expression<Func<TModel, int>> keyPropertyExpression)
        {
            var keyPropertyInfo = (PropertyInfo)(keyPropertyExpression.Body as MemberExpression).Member;
            var itemParameterExpression = Expression.Parameter(typeof(TModel));
            var newKeyParameterExpression = Expression.Parameter(typeof(int));

            var assignKeyExpression = Expression.Assign(Expression.Property(itemParameterExpression, keyPropertyInfo),
                newKeyParameterExpression);

            var lambda = Expression.Lambda<Action<TModel, int>>(assignKeyExpression, itemParameterExpression,
                newKeyParameterExpression);

            return lambda.Compile();
        }
    }
}
