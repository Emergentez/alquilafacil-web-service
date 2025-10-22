namespace AlquilaFacilPlatform.Subscriptions.Domain.Model.Commands;

public record CreateSubscriptionPaymentCommand(int Id, string Type, string Details);