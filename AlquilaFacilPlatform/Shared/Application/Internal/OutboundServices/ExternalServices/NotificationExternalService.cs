using AlquilaFacilPlatform.Notifications.Interfaces.ACL;

namespace AlquilaFacilPlatform.Shared.Application.Internal.OutboundServices.ExternalServices;

public class NotificationExternalService(INotificationsContextFacade notificationsContextFacade) : INotificationExternalService
{
    public async Task<int> CreateNotification(
        string title,
        string description,
        int userId
    )
    {
        return await notificationsContextFacade.CreateNotification(title, description, userId);
    }
}