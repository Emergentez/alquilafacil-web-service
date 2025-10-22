using AlquilaFacilPlatform.Subscriptions.Domain.Model.Aggregates;
using AlquilaFacilPlatform.Subscriptions.Interfaces.ACL;

namespace AlquilaFacilPlatform.Shared.Application.Internal.OutboundServices.ExternalServices;

public class SubscriptionExternalService(ISubscriptionContextFacade subscriptionContextFacade) : ISubscriptionExternalService
{
    public async Task<IEnumerable<Subscription>> GetSubscriptionByUserIdsList(List<int> userIdsList)
    {
        return await subscriptionContextFacade.GetSubscriptionByUserIdsList(userIdsList);
    }
    
    public async Task<string> GetSubscriptionStatusByUserId(int userId)
    {
        return await subscriptionContextFacade.GetSubscriptionStatusByUserId(userId);
    }
}