using DocCareWeb.Application.Notifications;

namespace DocCareWeb.Application.Services
{
    public abstract class BaseService
    {
        private readonly INotificator _notificator;

        protected BaseService(INotificator notificator)
        {
            _notificator = notificator;
        }

        protected void Notify(string message)
        {
            _notificator.AddNotification(new Notification(message));
        }

        protected bool IsValidOperation()
        {
            return !_notificator.HasNotification();
        }
    }
}
