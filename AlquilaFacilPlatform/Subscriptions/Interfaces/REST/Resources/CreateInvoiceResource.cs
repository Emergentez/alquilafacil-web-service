namespace AlquilaFacilPlatform.Subscriptions.Interfaces.REST.Resources;

public record CreateInvoiceResource(int SubscriptionId, float Amount, DateTime Date);