namespace AlquilaFacilPlatform.Profiles.Domain.Model.ValueObjects;

public record DocumentNumber(string NumberDocument)
{
    public DocumentNumber() : this(String.Empty)
    {
        
    }
}