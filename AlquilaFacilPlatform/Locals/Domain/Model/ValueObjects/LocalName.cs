namespace AlquilaFacilPlatform.Locals.Domain.Model.ValueObjects;

public record LocalName(string Value)
{
    public LocalName() : this(String.Empty)
    {
        
    }
}