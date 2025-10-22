namespace AlquilaFacilPlatform.Subscriptions.Domain.Model.Commands;

public record CreateSubscriptionCommand(int UserId,int PlanId, string VoucherImageUrl);