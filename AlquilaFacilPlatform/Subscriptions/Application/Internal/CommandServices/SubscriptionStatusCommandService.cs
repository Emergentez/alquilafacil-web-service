using AlquilaFacilPlatform.Shared.Domain.Repositories;
using AlquilaFacilPlatform.Subscriptions.Domain.Model.Commands;
using AlquilaFacilPlatform.Subscriptions.Domain.Model.Entities;
using AlquilaFacilPlatform.Subscriptions.Domain.Model.ValueObjects;
using AlquilaFacilPlatform.Subscriptions.Domain.Repositories;
using AlquilaFacilPlatform.Subscriptions.Domain.Services;

namespace AlquilaFacilPlatform.Subscriptions.Application.Internal.CommandServices;

public class SubscriptionStatusCommandService(ISubscriptionStatusRepository subscriptionStatusRepository, IUnitOfWork unitOfWork) : ISubscriptionStatusCommandService
{
    public async Task Handle(SeedSubscriptionStatusCommand command)
    {
        foreach (ESubscriptionStatus status in Enum.GetValues(typeof(ESubscriptionStatus)))
        {
            if (await subscriptionStatusRepository.ExistsBySubscriptionStatus(status)) continue;
            var subscriptionStatus = new SubscriptionStatus(status.ToString());
            await subscriptionStatusRepository.AddAsync(subscriptionStatus);
        }

        await unitOfWork.CompleteAsync();
    }

}