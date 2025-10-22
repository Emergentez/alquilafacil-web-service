

namespace AlquilaFacilPlatform.Subscriptions.Domain.Model.Entities;

public partial class SubscriptionStatus
{
    public int Id { get;  }
    public string Status { get; set; }
}

public partial class SubscriptionStatus
{
    public SubscriptionStatus(string status)
    {
        Status = status;
    }
}