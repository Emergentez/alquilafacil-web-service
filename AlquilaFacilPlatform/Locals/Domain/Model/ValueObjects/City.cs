namespace AlquilaFacilPlatform.Locals.Domain.Model.ValueObjects;

public record City(string Value)
{
    public City() : this(string.Empty)
    {
        
    }
}