using AlquilaFacilPlatform.Management.Domain.Model.Commands;
using AlquilaFacilPlatform.Management.Domain.Model.Entities;
using AlquilaFacilPlatform.Management.Domain.Model.ValueObjects;
using AlquilaFacilPlatform.Management.Domain.Repositories;
using AlquilaFacilPlatform.Management.Domain.Services;
using AlquilaFacilPlatform.Shared.Domain.Repositories;

namespace AlquilaFacilPlatform.Management.Application.Internal.CommandServices;

public class SeedSensorTypeCommandService(ISensorTypeRepository repository, IUnitOfWork unitOfWork): ISeedSensorTypeCommandService
{
    public async Task Handle(SeedSensorTypesCommand command)
    {
        foreach (ESensorTypes type in Enum.GetValues(typeof(ESensorTypes)))
        {
            if (await repository.ExistsSensorType(type)) continue;
            var sensorType = new SensorType(type.ToString());
            await repository.AddAsync(sensorType);
        }

        await unitOfWork.CompleteAsync();
    }
}