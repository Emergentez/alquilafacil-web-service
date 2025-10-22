using AlquilaFacilPlatform.Subscriptions.Domain.Model.Commands;

namespace AlquilaFacilPlatform.Subscriptions.Domain.Services;

public interface ISeedSubscriptionPlanCommandService
{
    Task Handle(SeedSubscriptionPlanCommand command);
}