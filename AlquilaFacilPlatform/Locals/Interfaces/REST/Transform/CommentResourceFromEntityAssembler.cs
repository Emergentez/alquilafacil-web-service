using AlquilaFacilPlatform.Locals.Domain.Model.Aggregates;
using AlquilaFacilPlatform.Locals.Interfaces.REST.Resources;

namespace AlquilaFacilPlatform.Locals.Interfaces.REST.Transform;

public static class CommentResourceFromEntityAssembler
{
    public static CommentResource ToResourceFromEntity(Comment entity)
    {
        return new CommentResource(entity.Id, entity.UserId, entity.LocalId, entity.CommentText, entity.CommentRating);
    }
}