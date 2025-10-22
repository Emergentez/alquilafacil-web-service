using AlquilaFacilPlatform.Notifications.Domain.Models.Aggregates;
using AlquilaFacilPlatform.Notifications.Domain.Models.Queries;
using AlquilaFacilPlatform.Notifications.Domain.Repositories;
using AlquilaFacilPlatform.Notifications.Domain.Services;

namespace AlquilaFacilPlatform.Notifications.Application.Internal.QueryServices;

public class NotificationQueryService(INotificationRepository notificationRepository) : INotificationQueryService
{
    public async Task<IEnumerable<Notification>> Handle(GetNotificationsByUserIdQuery query)
    {
        return await notificationRepository.GetNotificationsByUserId(query.UserId);
    }
}