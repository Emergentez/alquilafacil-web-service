namespace AlquilaFacilPlatform.Subscriptions.Domain.Model.Commands;

public record CreateInvoiceCommand(int SubscriptionId, float Amount, DateTime Date);