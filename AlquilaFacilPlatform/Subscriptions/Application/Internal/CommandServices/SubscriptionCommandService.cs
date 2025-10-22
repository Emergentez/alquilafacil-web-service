using AlquilaFacilPlatform.Shared.Application.Internal.OutboundServices;
using AlquilaFacilPlatform.Shared.Domain.Repositories;
using AlquilaFacilPlatform.Subscriptions.Domain.Model.Aggregates;
using AlquilaFacilPlatform.Subscriptions.Domain.Model.Commands;
using AlquilaFacilPlatform.Subscriptions.Domain.Repositories;
using AlquilaFacilPlatform.Subscriptions.Domain.Services;

namespace AlquilaFacilPlatform.Subscriptions.Application.Internal.CommandServices;

public class SubscriptionCommandService(ISubscriptionRepository subscriptionRepository, ISubscriptionStatusRepository subscriptionStatusRepository,
    IPlanRepository planRepository, 
    IUnitOfWork unitOfWork, IUserExternalService userExternalService)
    : ISubscriptionCommandService
{
    public async Task<Subscription?> Handle(CreateSubscriptionCommand command)
    {
        var subscription = new Subscription(command);
        var plan = await planRepository.FindByIdAsync(command.PlanId);
        if (plan == null)
        {
            throw new Exception("Plan not found");
        }

        if (!userExternalService.UserExists(command.UserId))
        {
            throw new Exception("User not found");
        }
        
        await subscriptionRepository.AddAsync(subscription);
        await unitOfWork.CompleteAsync();
        return subscription;
    }

    public async Task<Subscription?> Handle(ActiveSubscriptionStatusCommand command)
    {
        var subscription = await subscriptionRepository.FindByIdAsync(command.SubscriptionId);
        if (subscription == null)
        {
            throw new Exception("Subscription not found");
        }
        subscription.ActiveSubscriptionStatus();
        subscriptionRepository.Update(subscription);
        await unitOfWork.CompleteAsync();
        return subscription;
    }
}