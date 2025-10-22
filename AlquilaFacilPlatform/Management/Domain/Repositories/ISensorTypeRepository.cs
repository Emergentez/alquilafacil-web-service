using AlquilaFacilPlatform.Management.Domain.Model.Entities;
using AlquilaFacilPlatform.Management.Domain.Model.ValueObjects;
using AlquilaFacilPlatform.Shared.Domain.Repositories;

namespace AlquilaFacilPlatform.Management.Domain.Repositories;

public interface ISensorTypeRepository: IBaseRepository<SensorType>
{
    Task<bool> ExistsSensorType(ESensorTypes sensorType);
}