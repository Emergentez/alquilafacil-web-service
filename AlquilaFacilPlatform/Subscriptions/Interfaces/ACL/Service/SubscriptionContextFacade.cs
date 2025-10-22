using AlquilaFacilPlatform.Subscriptions.Domain.Model.Aggregates;
using AlquilaFacilPlatform.Subscriptions.Domain.Model.Queries;
using AlquilaFacilPlatform.Subscriptions.Domain.Model.ValueObjects;
using AlquilaFacilPlatform.Subscriptions.Domain.Services;

namespace AlquilaFacilPlatform.Subscriptions.Interfaces.ACL.Service;

public class SubscriptionContextFacade(ISubscriptionQueryServices subscriptionQueryServices) : ISubscriptionContextFacade
{
    

    public async Task<IEnumerable<Subscription>> GetSubscriptionByUserIdsList(List<int> userIdsList)
    {
        var query = new GetSubscriptionsByUserIdQuery(userIdsList);
        var subscriptions = await subscriptionQueryServices.Handle(query);  
        return subscriptions;
    }

    public async Task<string> GetSubscriptionStatusByUserId(int userId)
    {
        var query = new GetSubscriptionByUserIdQuery(userId);
        var subscription = await subscriptionQueryServices.Handle(query);
        if (subscription == null)
        {
            return "No subscription found";
        }
        return ((ESubscriptionStatus)subscription.SubscriptionStatusId).ToString();
    }
}