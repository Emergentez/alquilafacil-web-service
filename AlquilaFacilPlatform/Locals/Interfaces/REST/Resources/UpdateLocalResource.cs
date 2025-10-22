namespace AlquilaFacilPlatform.Locals.Interfaces.REST.Resources;

public record UpdateLocalResource(
    string LocalName,
    string DescriptionMessage,
    string Country,
    string City,
    string District,
    string Street,
    int Price,
    int Capacity,
    string Features,
    int LocalCategoryId,
    int UserId
);