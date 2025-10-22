namespace AlquilaFacilPlatform.Shared.Application.Internal.OutboundServices;

public interface INotificationExternalService
{
    Task<int> CreateNotification(
        string title,
        string description,
        int userId
    );
}