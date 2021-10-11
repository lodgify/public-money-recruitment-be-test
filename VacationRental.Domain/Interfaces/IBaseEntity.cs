using System;
using System.Collections.Generic;
using VacationRental.Domain.Base;

namespace VacationRental.Domain.Interfaces
{
    public interface IBaseEntity : ICloneable
    {
        public int Id { get; }
        public Guid Guid { get; set; }
        IReadOnlyList<BaseDomainEvent> Events { get; }
    }
}