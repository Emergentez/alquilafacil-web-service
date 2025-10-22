using AlquilaFacilPlatform.Subscriptions.Domain.Model.Commands;

namespace AlquilaFacilPlatform.Subscriptions.Domain.Services;

public interface ISubscriptionStatusCommandService
{
    Task Handle(SeedSubscriptionStatusCommand command);
}