using Flunt.Notifications;
using System.Collections.Generic;

namespace VacationRental.Application.Notifications
{
    public class EntityResult<T> : Result where T : class
    {
        public T Entity { get; }

        public EntityResult(IReadOnlyCollection<Notification> notifications, T entity)
            : base(notifications)
        {
            Entity = entity;
        }
    }
}
