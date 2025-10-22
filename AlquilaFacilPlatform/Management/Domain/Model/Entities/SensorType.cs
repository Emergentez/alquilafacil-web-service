namespace AlquilaFacilPlatform.Management.Domain.Model.Entities;

public class SensorType
{
    public SensorType()
    {
        Type = string.Empty;
    }
    
    public SensorType(string type)
    {
        Type = type;
    }
    
    public int Id { get; set; }
    public string Type { get; set; }
}