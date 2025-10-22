namespace AlquilaFacilPlatform.Subscriptions.Domain.Model.Aggregates;

public class Invoice
{
    public int Id { get; }
    public float Amount { get;  set; }
    public DateTime Date { get; set; }
    public int SubscriptionId { get; set; }

    public Invoice(int subscriptionId, float amount, DateTime date)
    {
        SubscriptionId = subscriptionId;
        Amount = amount;
        Date = date;
    }
}