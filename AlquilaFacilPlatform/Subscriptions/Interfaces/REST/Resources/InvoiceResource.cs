using AlquilaFacilPlatform.Subscriptions.Domain.Model.Aggregates;

namespace AlquilaFacilPlatform.Subscriptions.Interfaces.REST.Resources;

public record InvoiceResource(int SubscriptionId, float Amount, DateTime Date);