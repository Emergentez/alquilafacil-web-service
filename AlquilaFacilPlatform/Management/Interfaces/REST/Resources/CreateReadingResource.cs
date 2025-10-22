namespace AlquilaFacilPlatform.Management.Interfaces.REST.Resources;

public record CreateReadingResource(int LocalId, int SensorTypeId, string Message, DateTime Timestamp);