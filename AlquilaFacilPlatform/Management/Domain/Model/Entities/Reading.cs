using AlquilaFacilPlatform.Management.Domain.Model.Commands;
using AlquilaFacilPlatform.Management.Domain.Model.ValueObjects;

namespace AlquilaFacilPlatform.Management.Domain.Model.Entities;

public class Reading
{
    public Reading()
    {
        LocalId = 0;
        SensorTypeId = 0;
        Message = string.Empty;
        Timestamp = DateTime.UtcNow;
    }
    
    public Reading(int localId, int sensorTypeId, string message, DateTime timestamp)
    {
        LocalId = localId;
        SensorTypeId = sensorTypeId;
        Message = message;
        Timestamp = timestamp;
    }

    public Reading(CreateReadingCommand command)
    {
        LocalId = command.LocalId;
        SensorTypeId = command.SensorTypeId; 
        Message = command.Message;
        Timestamp = command.Timestamp;
    }

    public int Id { get; set; }
    public int LocalId { get; set; }
    public int SensorTypeId { get; set; }
    public string Message { get; set; } = string.Empty;
    public DateTime Timestamp { get; set; }
}