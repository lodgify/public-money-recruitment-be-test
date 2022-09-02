using Mapster;
using System;
using System.Linq.Expressions;

namespace VR.Infrastructure.Mapping
{
    public class MapConfig<TFrom, TTo>
    {
        private readonly TypeAdapterSetter<TFrom, TTo> _config;

        public MapConfig(int? maxDepth = null)
        {
            _config = TypeAdapterConfig<TFrom, TTo>.NewConfig().MaxDepth(maxDepth ?? 1);
        }

        public MapConfig<TFrom, TTo> ForMember<TFromProperty, TToProperty>(Expression<Func<TFrom, TFromProperty>> fromFunc, Expression<Func<TTo, TToProperty>> toMember,
                                                                           Expression<Func<TFrom, bool>> shouldMap = null)
        {
            _config.Map(toMember, fromFunc, shouldMap);
            return this;
        }

        public MapConfig<TFrom, TTo> FromMethod<TFromProperty, TToProperty>(Func<TFrom, TFromProperty> fromFunc, Expression<Func<TTo, TToProperty>> toMember)
        {
            _config.Map(toMember, from => fromFunc(from));
            return this;
        }

        public MapConfig<TFrom, TTo> ForMemberExpr<TFromProperty, TToProperty>(Expression<Func<TFrom, TFromProperty>> fromExpr, Expression<Func<TTo, TToProperty>> toMember)
        {
            _config.Map(toMember, fromExpr);
            return this;
        }

        public MapConfig<TFrom, TTo> ForMemberToTargetWith(Expression<Func<TFrom, TTo, TTo>> toMember)
        {
            _config.MapToTargetWith(toMember);
            return this;
        }

        public MapConfig<TFrom, TTo> ForMemberWith(Expression<Func<TFrom, TTo>> toMember)
        {
            _config.MapWith(toMember);
            return this;
        }

        public MapConfig<TFrom, TTo> ConstructUsing(Expression<Func<TFrom, TTo>> toMember)
        {
            _config.ConstructUsing(toMember);
            return this;
        }

        public MapConfig<TFrom, TTo> Ignore(Expression<Func<TTo, object>> toMember)
        {
            _config.Ignore(toMember);
            return this;
        }

        public void AfterMapping(Action<TFrom, TTo> completeWith) => _config.AfterMapping(completeWith);
    }
}
