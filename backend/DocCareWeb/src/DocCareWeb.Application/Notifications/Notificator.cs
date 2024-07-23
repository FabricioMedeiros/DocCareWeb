namespace DocCareWeb.Application.Notifications
{
    public class Notificator : INotificator
    {
        private List<Notification> _notifications;

        public Notificator()
        {
            _notifications = new List<Notification>();
        }

        public List<Notification> GetNotifications()
        {
            return _notifications;
        }

        public void AddNotification(Notification notification)
        {
            _notifications.Add(notification);
        }

        public bool HasNotification()
        {
            return _notifications.Any();
        }
    }
}
