using AlquilaFacilPlatform.Shared.Domain.Repositories;
using AlquilaFacilPlatform.Subscriptions.Domain.Model.Commands;
using AlquilaFacilPlatform.Subscriptions.Domain.Repositories;
using AlquilaFacilPlatform.Subscriptions.Domain.Services;

namespace AlquilaFacilPlatform.Subscriptions.Application.Internal.CommandServices;

public class SeedSubscriptionPlanCommandService(IPlanRepository repository, IPlanCommandService commandService, IUnitOfWork unitOfWork): ISeedSubscriptionPlanCommandService
{
    public async Task Handle(SeedSubscriptionPlanCommand command)
    {
        var existingPlan = await repository.FindByIdAsync(1);
        if (existingPlan != null) return;
        var planCommand = new CreatePlanCommand("Plan Premium",
            "El plan premium te permitirá acceder a funcionalidades adicionales en la aplicación", 20);
        await commandService.Handle(planCommand);
        await unitOfWork.CompleteAsync();
    }
}
