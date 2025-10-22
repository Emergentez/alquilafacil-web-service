using AlquilaFacilPlatform.Subscriptions.Domain.Model.Aggregates;

namespace AlquilaFacilPlatform.Subscriptions.Interfaces.ACL;

public interface ISubscriptionContextFacade
{
    Task<IEnumerable<Subscription>> GetSubscriptionByUserIdsList(List<int> userIdsList);
    Task<string> GetSubscriptionStatusByUserId(int userId);
}