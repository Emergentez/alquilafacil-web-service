using AlquilaFacilPlatform.IAM.Domain.Model.Commands;
using AlquilaFacilPlatform.IAM.Interfaces.REST.Resources;

namespace AlquilaFacilPlatform.IAM.Interfaces.REST.Transform;

public static class UpdateUsernameCommandFromResourceAssembler
{
    public static UpdateUsernameCommand ToUpdateUsernameCommand(int id, UpdateUsernameResource resource)
    {
        return new UpdateUsernameCommand(id, resource.Username);
    }
}