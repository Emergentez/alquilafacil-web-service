namespace AlquilaFacilPlatform.Locals.Domain.Model.ValueObjects;

public record PricePerHour(int Value)
{
    public PricePerHour() : this(0)
    {
        
    }
}