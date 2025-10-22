namespace AlquilaFacilPlatform.Management.Domain.Model.Commands;

public record CreateReadingCommand(int LocalId, int SensorTypeId, string Message, DateTime Timestamp);