﻿using System.Threading;
using System.Threading.Tasks;

namespace VacationRental.Shared.Abstractions.Queries
{
    public interface IQueryHandler<in TQuery, TResult> where TQuery : class, IQuery<TResult>
    {
        Task<TResult> HandleAsync(TQuery query, CancellationToken cancellationToken = default);
    }
}
