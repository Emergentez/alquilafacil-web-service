namespace AlquilaFacilPlatform.Subscriptions.Interfaces.REST.Resources;

public record CreateSubscriptionResource(int PlanId, int UserId, string VoucherImageUrl);