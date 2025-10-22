namespace AlquilaFacilPlatform.Locals.Domain.Model.Entities;

public class LocalCategory
{
    public LocalCategory()
    {
        Name = string.Empty;
        PhotoUrl = string.Empty;
    }

    public LocalCategory(string name, string photoUrl)
    {
        Name = name;
        PhotoUrl = photoUrl;
    }

    public int Id { get; set; }
    public string Name { get; set; }
    public string PhotoUrl { get; set; } 
}
