using AlquilaFacilPlatform.Shared.Domain.Repositories;
using AlquilaFacilPlatform.Subscriptions.Domain.Model.Aggregates;

namespace AlquilaFacilPlatform.Subscriptions.Domain.Repositories;

public interface ISubscriptionRepository : IBaseRepository<Subscription>
{
    Task<Subscription?> FindByUserIdAsync(int userId);
    Task<IEnumerable<Subscription>> FindByUserIdsListAsync(List<int> userIdsList);
}