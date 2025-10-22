using AlquilaFacilPlatform.Management.Domain.Model.Entities;
using AlquilaFacilPlatform.Management.Interfaces.REST.Resources;

namespace AlquilaFacilPlatform.Management.Interfaces.REST.Transform;

public static class ReadingResourceFromEntityAssembler
{
    public static ReadingResource ToResourceFromEntity(Reading reading)
    {
        return new ReadingResource(
            reading.Id,
            reading.LocalId,
            reading.SensorTypeId,
            reading.Message,
            reading.Timestamp
        );
    }
}