namespace DChild.Gameplay.UI
{
    public interface INotificationHandle
    {
        int priority { get; }
        bool HasNotifications();
        void HandleNextNotification();
        void RemoveAllQueuedNotifications();
    }

}