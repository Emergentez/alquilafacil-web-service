namespace AlquilaFacilPlatform.Notifications.Interfaces.ACL;

public interface INotificationsContextFacade
{
    public Task<int> CreateNotification(
        string title,
        string description,
        int userId
    );
}