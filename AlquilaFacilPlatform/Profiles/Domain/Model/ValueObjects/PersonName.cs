namespace AlquilaFacilPlatform.Profiles.Domain.Model.ValueObjects;

public record PersonName(string Name, string? FatherName, string? MotherName)
{
    public PersonName() : this(string.Empty, string.Empty, string.Empty)
    {
    }

    public PersonName(string name) : this(name, string.Empty, string.Empty)
    {
    }
    
    public PersonName(string name, string? fatherName) : this(name, fatherName, string.Empty)
    {
    }

    public string FullName => $"{Name} {FatherName} {MotherName}";

}