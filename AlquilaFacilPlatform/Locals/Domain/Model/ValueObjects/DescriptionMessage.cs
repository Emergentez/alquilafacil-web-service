namespace AlquilaFacilPlatform.Locals.Domain.Model.ValueObjects;

public record DescriptionMessage(string Value)
{
    public DescriptionMessage() : this(string.Empty)
    {
        
    }
}