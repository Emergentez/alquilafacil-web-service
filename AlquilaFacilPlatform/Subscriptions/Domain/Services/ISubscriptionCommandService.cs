using AlquilaFacilPlatform.Subscriptions.Domain.Model.Aggregates;
using AlquilaFacilPlatform.Subscriptions.Domain.Model.Commands;

namespace AlquilaFacilPlatform.Subscriptions.Domain.Services;

public interface ISubscriptionCommandService
{
    public Task<Subscription?> Handle(CreateSubscriptionCommand command);

    public Task<Subscription?> Handle(ActiveSubscriptionStatusCommand command);
}