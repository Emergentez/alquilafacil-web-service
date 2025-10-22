namespace AlquilaFacilPlatform.Profiles.Domain.Model.ValueObjects;

public record Phone(string PhoneNumber)
{
    public Phone() : this(String.Empty)
    {
        
    }
}