using AlquilaFacilPlatform.Shared.Domain.Repositories;
using AlquilaFacilPlatform.Subscriptions.Domain.Model.Entities;
using AlquilaFacilPlatform.Subscriptions.Domain.Model.ValueObjects;

namespace AlquilaFacilPlatform.Subscriptions.Domain.Repositories;

public interface ISubscriptionStatusRepository : IBaseRepository<SubscriptionStatus>
{
    Task<bool> ExistsBySubscriptionStatus(ESubscriptionStatus subscriptionStatus);
}