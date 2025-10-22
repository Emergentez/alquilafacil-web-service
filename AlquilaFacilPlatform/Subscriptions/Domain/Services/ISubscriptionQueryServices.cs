using AlquilaFacilPlatform.Subscriptions.Domain.Model.Aggregates;
using AlquilaFacilPlatform.Subscriptions.Domain.Model.Queries;

namespace AlquilaFacilPlatform.Subscriptions.Domain.Services;

public interface ISubscriptionQueryServices
{
    Task<Subscription?> Handle(GetSubscriptionByIdQuery query);
    Task<IEnumerable<Subscription>> Handle(GetAllSubscriptionsQuery query);
    
    Task<Subscription?> Handle(GetSubscriptionByUserIdQuery query);
    Task<IEnumerable<Subscription>> Handle(GetSubscriptionsByUserIdQuery query);
}