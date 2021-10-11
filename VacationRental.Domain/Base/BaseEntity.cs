using System;
using System.Collections.Generic;
using VacationRental.Domain.Interfaces;

namespace VacationRental.Domain.Base
{
    public abstract class BaseEntity : IBaseEntity
    {
        private List<BaseDomainEvent> _events { get; } = new List<BaseDomainEvent>();
        public IReadOnlyList<BaseDomainEvent> Events => _events.AsReadOnly();

        public Guid Guid { get; set; } = Guid.NewGuid();
        public int Id
        {
            get
            {
                return BitConverter.ToInt32(Guid.ToByteArray(), 0);
            }
        }

        protected void AddEvent(BaseDomainEvent @event)
        {
            _events.Add(@event);
        }

        protected void RemoveEvent(BaseDomainEvent @event)
        {
            _events.Remove(@event);
        }

        public object Clone()
        {
            object cloned = this.HandleClone();
            ((BaseEntity)cloned).Update(this.Guid);
            return cloned;
        }
        public abstract object HandleClone();

        public void Update(Guid guid)
        {
            this.Guid = guid;
        }


    }
}