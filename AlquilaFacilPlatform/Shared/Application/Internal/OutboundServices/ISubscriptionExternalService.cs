using AlquilaFacilPlatform.Subscriptions.Domain.Model.Aggregates;

namespace AlquilaFacilPlatform.Shared.Application.Internal.OutboundServices;

public interface ISubscriptionExternalService
{
    Task<IEnumerable<Subscription>> GetSubscriptionByUserIdsList(List<int> usersId);
    Task<string> GetSubscriptionStatusByUserId(int userId);
}