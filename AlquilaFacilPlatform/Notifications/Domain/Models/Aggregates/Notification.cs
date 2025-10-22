using AlquilaFacilPlatform.Notifications.Domain.Models.Commands;

namespace AlquilaFacilPlatform.Notifications.Domain.Models.Aggregates;

public partial class Notification
{
    public int Id { get; set; }
    public string Title { get; }
    public string Description { get; }
    public int UserId { get; }
}

public partial class Notification
{
    public Notification()
    {
        Title = string.Empty;
        Description = string.Empty;
        UserId = 0;
    }

    public Notification(CreateNotificationCommand command)
    {
        Title = command.Title;
        Description = command.Description;
        UserId = command.UserId;
    }
}