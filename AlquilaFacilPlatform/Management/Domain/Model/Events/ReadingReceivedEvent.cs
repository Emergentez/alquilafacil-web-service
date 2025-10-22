namespace AlquilaFacilPlatform.Management.Domain.Model.Events;

public record ReadingReceivedEvent(int LocalId, int SensorTypeId, string Message);