using Flunt.Notifications;
using System.Collections.Generic;

namespace VacationRental.Application.Notifications
{
    public class Result : Notifiable
    {
        public Result(IReadOnlyCollection<Notification> notifications)
        {
            this.AddNotifications(notifications);
        }

        public void AddNotification(string error)
        {
            this.AddNotification(null, error);
        }

        public void AddNotification(string error, ErrorCode errorCode)
        {
            this.AddNotification(null, error);
            this.Error = errorCode;
        }

        public void AddNotification(string property, string message, ErrorCode errorCode)
        {
            this.AddNotification(property, message);
            this.Error = errorCode;
        }

        public void AddNotification(Notification notification, ErrorCode errorCode)
        {
            this.AddNotification(notification);
            this.Error = errorCode;
        }

        public void AddNotifications(IReadOnlyCollection<Notification> notifications, ErrorCode errorCode)
        {
            this.AddNotifications(notifications);
            this.Error = errorCode;
        }

        public ErrorCode? Error { get; set; }
    }
}

