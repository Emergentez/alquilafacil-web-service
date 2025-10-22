using AlquilaFacilPlatform.Notifications.Domain.Models.Commands;
using AlquilaFacilPlatform.Notifications.Interfaces.REST.Resources;

namespace AlquilaFacilPlatform.Notifications.Interfaces.REST.Transforms;

public static class CreateNotificationCommandFromResourceAssembler
{
    public static CreateNotificationCommand ToCommandFromResource(CreateNotificationResource createNotificationResource)
    {
        return new CreateNotificationCommand(
            createNotificationResource.Title,
            createNotificationResource.Description,
            createNotificationResource.UserId
            );
    }
}