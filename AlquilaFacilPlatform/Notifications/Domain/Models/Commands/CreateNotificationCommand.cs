namespace AlquilaFacilPlatform.Notifications.Domain.Models.Commands;

public record CreateNotificationCommand(string Title, string Description, int UserId);