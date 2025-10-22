namespace AlquilaFacilPlatform.Management.Interfaces.REST.Resources;

public record ReadingResource(int Id, int LocalId, int SensorTypeId, string Message, DateTime Timestamp);