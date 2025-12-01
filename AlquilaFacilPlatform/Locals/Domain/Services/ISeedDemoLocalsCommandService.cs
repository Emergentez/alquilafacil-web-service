using AlquilaFacilPlatform.Locals.Domain.Model.Commands;

namespace AlquilaFacilPlatform.Locals.Domain.Services;

public interface ISeedDemoLocalsCommandService
{
    Task Handle(SeedDemoLocalsCommand command);
}
