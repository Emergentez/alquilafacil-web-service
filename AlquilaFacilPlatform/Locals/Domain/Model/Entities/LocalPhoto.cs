using AlquilaFacilPlatform.Locals.Domain.Model.Aggregates;

namespace AlquilaFacilPlatform.Locals.Domain.Model.Entities;

public class LocalPhoto
{
    public LocalPhoto()
    {
        Url = string.Empty;
    }
    
    public LocalPhoto(string url)
    {
        Url = url;
    }
    
    public int Id { get; set; }
    public string Url { get; set; }
    public int LocalId { get; set; }
    
    public Local Local { get; set; }
}