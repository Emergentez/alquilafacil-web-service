using AlquilaFacilPlatform.Management.Domain.Model.Commands;

namespace AlquilaFacilPlatform.Management.Domain.Services;

public interface ISeedSensorTypeCommandService
{
    Task Handle(SeedSensorTypesCommand command);
}