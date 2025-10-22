using AlquilaFacilPlatform.Shared.Infrastructure.Persistence.EFC.Configuration;
using AlquilaFacilPlatform.Shared.Infrastructure.Persistence.EFC.Repositories;
using AlquilaFacilPlatform.Subscriptions.Domain.Model.Entities;
using AlquilaFacilPlatform.Subscriptions.Domain.Model.ValueObjects;
using AlquilaFacilPlatform.Subscriptions.Domain.Repositories;
using Microsoft.EntityFrameworkCore;

namespace AlquilaFacilPlatform.Subscriptions.Infrastructure.Persistence.EFC.Repositories;

public class SubscriptionStatusRepository(AppDbContext context) : BaseRepository<SubscriptionStatus>(context), ISubscriptionStatusRepository
{
    public async Task<bool> ExistsBySubscriptionStatus(ESubscriptionStatus subscriptionStatus)
    {
        return await Context.Set<SubscriptionStatus>().AnyAsync(x => x.Status == subscriptionStatus.ToString());
    }
}