using AlquilaFacilPlatform.Locals.Domain.Model.Commands;
using AlquilaFacilPlatform.Locals.Interfaces.REST.Resources;

namespace AlquilaFacilPlatform.Locals.Interfaces.REST.Transform;

public static class CreateCommentCommandFromResourceAssembler
{
    public static CreateCommentCommand ToCommandFromResource(CreateCommentResource resource)
    {
        return new CreateCommentCommand(resource.UserId, resource.LocalId, resource.Text, resource.Rating);
    }
}