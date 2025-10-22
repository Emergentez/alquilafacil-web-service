using AlquilaFacilPlatform.Locals.Domain.Model.Commands;
using AlquilaFacilPlatform.Locals.Interfaces.REST.Resources;

namespace AlquilaFacilPlatform.Locals.Interfaces.REST.Transform;

public static class UpdateLocalCommandFromResourceAssembler
{
    public static UpdateLocalCommand ToCommandFromResource(int id, UpdateLocalResource resource)
    {
        return new UpdateLocalCommand(
            id,
            resource.LocalName,
            resource.DescriptionMessage,
            resource.Country,
            resource.City,
            resource.District,
            resource.Street,
            resource.Price,
            resource.Capacity,
            resource.Features,
            resource.LocalCategoryId,
            resource.UserId
            );
    }
}