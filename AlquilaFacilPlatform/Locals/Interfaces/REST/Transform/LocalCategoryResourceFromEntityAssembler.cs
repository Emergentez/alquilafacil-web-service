using AlquilaFacilPlatform.Locals.Domain.Model.Entities;
using AlquilaFacilPlatform.Locals.Interfaces.REST.Resources;

namespace AlquilaFacilPlatform.Locals.Interfaces.REST.Transform;

public static class LocalCategoryResourceFromEntityAssembler
{
    public static LocalCategoryResource ToResourceFromEntity(LocalCategory localCategory)
    {
        return new LocalCategoryResource(
            localCategory.Id,
            localCategory.Name,
            localCategory.PhotoUrl
            );
    }
}
